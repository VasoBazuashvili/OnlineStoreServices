using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Interfaces
{
	public interface IUnitOfWork
	{
		Task BeginTransactionAsync(CancellationToken ct);
		Task CommitTransactionAsync(CancellationToken ct);
		Task RollbackTransactionAsync(CancellationToken ct);
		Task SaveChangesAsync(CancellationToken ct = default);
	}
}
