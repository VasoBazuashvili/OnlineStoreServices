using MediatR;
using ProductCatalogService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Commands.ProductCatalog.UpdateProduct
{
	public record UpdateProductCommand(
	int Id,
	string SKU,
	decimal Price,
	int StockQuantity,
	bool IsActive
	) : IRequest<Response<Unit>>;
}
