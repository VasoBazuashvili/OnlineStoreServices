using OrderService.Shared.DomainUtilities;
using OrdersService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Interfaces
{
	public interface IOrderRepository : IBaseRepository<Order>
	{
		Task<IReadOnlyList<Order>> GetByUserAsync(int userId, int page, int size, CancellationToken cancellationToken = default); 
		Task<int> CountByUserAsync(int userId, CancellationToken cancellationToken = default);
	}
}
