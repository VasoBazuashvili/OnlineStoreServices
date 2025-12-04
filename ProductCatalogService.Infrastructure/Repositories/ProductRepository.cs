using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Infrastructure.Repositories
{
	public class ProductRepository : BaseRepository<ProductDbContext, Product>, IProductRepository
	{

		private readonly ProductDbContext _db;

		public ProductRepository(ProductDbContext db)
			: base(db)
		{
			_db = db;
		}

		public async Task<List<Product>> GetByIdsAsync(IEnumerable<int> ids)
		=> await _db.Products
					.Where(p => ids.Contains(p.Id))
					.ToListAsync();

		public async Task<IEnumerable<Product>> GetProductsAsync(int page, int size)
			=> await _db.Products
						.Skip((page - 1) * size)
						.Take(size)
						.ToListAsync();

		public async Task<int> CountAsync() =>
			await _db.Products.CountAsync();
	}
}
