using TranslationTabsDemo.Data.Domain.Common;

namespace TranslationTabsDemo.Data.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
}