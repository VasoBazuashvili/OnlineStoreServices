using Microsoft.EntityFrameworkCore;
using OrdersService.Application.Interfaces;
using OrdersService.Domain.Entities;
using OrdersService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Infrastructure.Repositories
{
	public class IdempotencyRepository : BaseRepository<OrdersDbContext,IdempotencyRequest>, IIdempotencyRepository
	{
		public IdempotencyRepository(OrdersDbContext db)
			: base(db)
		{
		}
		public async Task<IdempotencyRequest?> GetByKeyAsync(string key, CancellationToken ct)
		{
			return await _dbSet
				.SingleOrDefaultAsync(r => r.Key == key, ct);
		}
	}
}
