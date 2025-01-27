using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TranslationTabsDemo.Data.Domain.Common;

namespace TranslationTabsDemo.Data.Domain.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    DbSet<TEntity> Entities { get; }
    Task<TEntity?> GetFirstAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes);

    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes);

    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}