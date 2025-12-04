using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Interfaces
{
	public interface IUnitOfWork
	{
		Task BeginTransactionAsync(CancellationToken cancellationToken = default);
		Task CommitAsync(CancellationToken cancellationToken = default);
		Task RollbackAsync(CancellationToken cancellationToken = default);
		Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
