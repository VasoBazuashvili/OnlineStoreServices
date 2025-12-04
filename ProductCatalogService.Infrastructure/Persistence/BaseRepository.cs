using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Shared.DomainUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Infrastructure.Persistence
{
	public class BaseRepository<TContext, TEntity> : IRepository<TEntity>
	  where TEntity : class
	  where TContext : DbContext
	{
		protected readonly TContext _context;

		public BaseRepository(TContext context) =>
			_context = context;

		public async Task<TEntity?> GetByIdAsync(int id) =>
			await _context.Set<TEntity>().FindAsync(id);

		public Task AddAsync(TEntity entity) =>
			_context.Set<TEntity>().AddAsync(entity).AsTask();

		public void Update(TEntity entity) =>
			_context.Entry(entity).State = EntityState.Modified;

		public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? predicate = null) =>
			predicate == null ? _context.Set<TEntity>() : _context.Set<TEntity>().Where(predicate);

		public void Remove(TEntity entity) =>
			_context.Set<TEntity>().Remove(entity);
	}

}
