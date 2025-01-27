using TranslationTabsDemo.Data.Domain.Common;
using TranslationTabsDemo.Data.Domain.Repositories;
using TranslationTabsDemo.Data.Infrastructure.Context;

namespace TranslationTabsDemo.Data.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();

    public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IBaseRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        var repository = new BaseRepository<TEntity>(context);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public void Dispose()
    {
        context.Dispose();
    }
}