using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TranslationTabsDemo.Data.Domain.Common;
using TranslationTabsDemo.Data.Domain.Entities;
using TranslationTabsDemo.Data.Domain.Entities.Translations;

namespace TranslationTabsDemo.Data.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<BlogTranslation> BlogTranslations => Set<BlogTranslation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker.Entries()
            .Where(x => x is { Entity: BaseEntity, State: EntityState.Added or EntityState.Modified });
        foreach (var entry in entries)
        {
            ((BaseEntity)entry.Entity).UpdatedAt = DateTimeOffset.Now;

            if (entry.State == EntityState.Added)
            {
                ((BaseEntity)entry.Entity).CreatedAt = DateTimeOffset.Now;
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}