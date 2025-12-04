using Microsoft.EntityFrameworkCore;
using OrdersService.Application.Interfaces;
using OrdersService.Domain.Entities;
using OrdersService.Infrastructure.Persistence;

public class OrdersRepository : BaseRepository<OrdersDbContext, Order>, IOrderRepository
{
	public OrdersRepository(OrdersDbContext db)
		: base(db) 
	{
	}

	public override async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
	{
		return await _dbSet
			.Include(o => o.Items)
			.SingleOrDefaultAsync(o => o.Id == id, ct);
	}

	public async Task<IEnumerable<Order>> GetByUserAsync(int userId, int page, int size, CancellationToken ct)
	{
		return await _dbSet
			.Where(o => o.UserId == userId)
			.OrderByDescending(o => o.CreatedAt)
			.Skip((page - 1) * size)
			.Take(size)
			.Include(o => o.Items)
			.ToListAsync(ct);
	}

	public async Task<int> CountByUserAsync(int userId, CancellationToken ct = default)
	{
		return await _dbSet
			.Where(o => o.UserId == userId)
			.CountAsync(ct);
	}

	public override async Task AddAsync(Order entity, CancellationToken cancellationToken = default)
	{
		await _dbSet.AddAsync(entity, cancellationToken);
	}

	public override Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
	{
		_context.Entry(entity).State = EntityState.Modified;
		return Task.CompletedTask;
	}

	Task<IReadOnlyList<Order>> IOrderRepository.GetByUserAsync(int userId, int page, int size, CancellationToken cancellationToken)
	{
		return GetByUserAsync(userId, page, size, cancellationToken)
			.ContinueWith(t => (IReadOnlyList<Order>)t.Result.ToList(), cancellationToken);
	}
}
