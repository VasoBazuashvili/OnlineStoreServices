using Microsoft.EntityFrameworkCore;
using OrderService.Shared.DomainUtilities;

public class BaseRepository<TContext, TEntity> : IBaseRepository<TEntity>
	where TEntity : class
	where TContext : DbContext
{
	protected readonly TContext _context;
	protected readonly DbSet<TEntity> _dbSet;

	public BaseRepository(TContext context)
	{
		_context = context;
		_dbSet = _context.Set<TEntity>();
	}

	public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
	}

	public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await _dbSet.ToListAsync(cancellationToken);
	}

	public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		await _dbSet.AddAsync(entity, cancellationToken);
	}

	public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		_context.Entry(entity).State = EntityState.Modified;
		return Task.CompletedTask;
	}

	public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		_dbSet.Remove(entity);
		return Task.CompletedTask;
	}

	public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
	{
		return await _dbSet.CountAsync(cancellationToken);
	}
}
