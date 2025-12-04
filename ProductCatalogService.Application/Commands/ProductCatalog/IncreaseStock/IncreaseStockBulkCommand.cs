using MediatR;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Commands.ProductCatalog.IncreaseStock
{
	public record IncreaseStockBulkCommand(List<ProductQuantityDto> Items)
	: IRequest<Response<string>>;
}
