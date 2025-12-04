using Microsoft.EntityFrameworkCore.Storage;
using OrdersService.Application.Interfaces;
using OrdersService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
	private readonly OrdersDbContext _db;
	private IDbContextTransaction? _transaction;

	public UnitOfWork(OrdersDbContext db) => _db = db;

	public async Task BeginTransactionAsync(CancellationToken ct)
	{
		if (_transaction == null)
			_transaction = await _db.Database.BeginTransactionAsync(ct);
	}

	public async Task SaveChangesAsync(CancellationToken ct = default)
	{
		await _db.SaveChangesAsync(ct);
	}

	public async Task CommitTransactionAsync(CancellationToken ct)
	{
		if (_transaction != null)
		{
			await _db.SaveChangesAsync(ct);
			await _transaction.CommitAsync(ct);
			await _transaction.DisposeAsync();
			_transaction = null;
		}
	}

	public async Task RollbackTransactionAsync(CancellationToken ct)
	{
		if (_transaction != null)
		{
			await _transaction.RollbackAsync(ct);
			await _transaction.DisposeAsync();
			_transaction = null;
		}
	}
}
