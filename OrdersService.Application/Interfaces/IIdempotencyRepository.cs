using OrderService.Shared.DomainUtilities;
using OrdersService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Interfaces
{
	public interface IIdempotencyRepository : IBaseRepository<IdempotencyRequest>
	{
		Task<IdempotencyRequest?> GetByKeyAsync(string key, CancellationToken ct);
	}
}
