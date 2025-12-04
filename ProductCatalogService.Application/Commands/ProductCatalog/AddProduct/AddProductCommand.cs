using MediatR;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Commands.ProductCatalog.AddProduct
{
	public record AddProductCommand(
		string Name,
		string SKU,
		decimal Price,
		int StockQuantity,
		bool IsActive
	) : IRequest<Response<ProductDto>>;
}
