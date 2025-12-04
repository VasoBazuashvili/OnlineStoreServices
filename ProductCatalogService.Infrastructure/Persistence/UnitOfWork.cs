using Microsoft.EntityFrameworkCore.Storage;
using ProductCatalogService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Infrastructure.Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ProductDbContext _db;
		private IDbContextTransaction? _currentTransaction;

		public UnitOfWork(ProductDbContext db)
		{
			_db = db;
		}

		public async Task BeginTransactionAsync(CancellationToken ct = default)
		{
			_currentTransaction = await _db.Database.BeginTransactionAsync(ct);
		}

		public async Task CommitAsync(CancellationToken ct = default)
		{
			if (_currentTransaction != null)
				await _currentTransaction.CommitAsync(ct);
		}

		public async Task RollbackAsync(CancellationToken ct = default)
		{
			if (_currentTransaction != null)
				await _currentTransaction.RollbackAsync(ct);
		}

		public async Task SaveChangesAsync(CancellationToken ct = default)
		{
			await _db.SaveChangesAsync(ct);
		}
	}
}
