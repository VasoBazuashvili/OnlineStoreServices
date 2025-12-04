using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Shared.DomainUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Interfaces
{
	public interface IProductRepository : IRepository<Product>
	{
		Task<List<Product>> GetByIdsAsync(IEnumerable<int> ids);
		Task<IEnumerable<Product>> GetProductsAsync(int pageNumber, int pageSize);
		Task<int> CountAsync();
	}
}
