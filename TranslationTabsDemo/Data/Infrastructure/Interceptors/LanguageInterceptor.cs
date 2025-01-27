using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TranslationTabsDemo.Data.Domain.Common;
using TranslationTabsDemo.Data.Domain.Entities;
using TranslationTabsDemo.Data.Domain.Entities.Translations;

namespace TranslationTabsDemo.Data.Infrastructure.Interceptors;

public class LanguageInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context == null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var addedEntities = context.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added && x.Entity is Language)
            .ToList();

        foreach (var entityEntry in addedEntities)
        {
            if (entityEntry.Entity is not Language language) continue;

            await AddTranslation<Blog, BlogTranslation>(context, language.Code, cancellationToken);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static async Task AddTranslation<TEntity, TTranslation>(
        DbContext context,
        string? languageCode,
        CancellationToken cancellationToken)
        where TEntity : BaseEntity
        where TTranslation : BaseEntity, new()
    {
        var entities = await context.Set<TEntity>().ToListAsync(cancellationToken);

        foreach (var entity in entities)
        {
            var translation = new TTranslation();
            var translationType = typeof(TTranslation);

            foreach (var property in translationType.GetProperties())
            {
                if (property.Name.Equals("LanguageCode", StringComparison.OrdinalIgnoreCase))
                {
                    property.SetValue(translation, languageCode);
                }
                else if (property.Name.Equals($"{typeof(TEntity).Name}Id", StringComparison.OrdinalIgnoreCase))
                {
                    property.SetValue(translation, entity.Id);
                }
            }

            context.Add(translation);
        }
    }
}