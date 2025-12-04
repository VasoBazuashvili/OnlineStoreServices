using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Shared.DomainUtilities
{
	public interface IRepository<TEntity> where TEntity : class
	{
		Task<TEntity?> GetByIdAsync(int id);
		Task AddAsync(TEntity entity);
		void Update(TEntity entity);
		IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? predicate = null);
		void Remove(TEntity entity);
	}
}
