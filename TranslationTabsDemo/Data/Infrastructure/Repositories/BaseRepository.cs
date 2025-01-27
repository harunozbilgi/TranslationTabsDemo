using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TranslationTabsDemo.Data.Domain.Common;
using TranslationTabsDemo.Data.Domain.Repositories;
using TranslationTabsDemo.Data.Infrastructure.Context;

namespace TranslationTabsDemo.Data.Infrastructure.Repositories;

public class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    private DbSet<TEntity> _entities => context.Set<TEntity>();

    public DbSet<TEntity> Entities => _entities;

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        await _entities.AddAsync(entity, cancellationToken);

        return entity;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _entities.Remove(entity);

        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        return await _entities.AnyAsync(predicate, cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes)
    {
        var query = _entities.AsQueryable();

        if (includes != null)
        {
            query = ApplyIncludes(query, includes);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        var query = _entities.AsQueryable();
        if (includes != null)
        {
            query = ApplyIncludes(query, includes);
        }

        return await query.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _entities.FindAsync([id], cancellationToken);
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        var query = _entities.AsQueryable();
        if (includes != null)
        {
            query = ApplyIncludes(query, includes);
        }

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        _entities.Update(entity);
        return Task.FromResult(entity);
    }

    private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes)
    {
        return includes != null ? includes.Aggregate(query, (current, include) => include(current)) : query;
    }

    public async Task<TEntity?> GetFirstAsync(CancellationToken cancellationToken = default)
    {
        return await _entities.FirstOrDefaultAsync(cancellationToken);
    }
}