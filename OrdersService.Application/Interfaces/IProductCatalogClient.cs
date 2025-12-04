using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Interfaces
{
	public record ProductQuantity(int ProductId, int Quantity);

	public record ReducedProductDto(int ProductId, string Name, decimal UnitPrice, int ReducedBy, int RemainingStock);

	public interface IProductCatalogClient
	{
		Task<IReadOnlyList<ReducedProductDto>> ReduceStockBulkAsync(IEnumerable<ProductQuantity> items, string bearerToken, CancellationToken ct);
		Task IncreaseStockBulkAsync(IEnumerable<ProductQuantity> items, string bearerToken, CancellationToken ct);
		Task<ProductDetailDto?> GetProductByIdAsync(int productId, string bearerToken, CancellationToken ct);
	}

	public record ProductDetailDto(int ProductId, string Name, decimal UnitPrice, int StockQuantity, bool IsActive);
}
