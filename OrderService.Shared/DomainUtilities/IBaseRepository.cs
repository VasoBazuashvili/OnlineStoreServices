using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Shared.DomainUtilities
{
	public interface IBaseRepository<TEntity> where TEntity : class
	{
		Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
		Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
		Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
		Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
		Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
		Task<int> CountAsync(CancellationToken cancellationToken = default);
	}
}
