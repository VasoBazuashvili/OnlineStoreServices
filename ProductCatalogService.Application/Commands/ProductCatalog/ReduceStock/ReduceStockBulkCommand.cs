using MediatR;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Commands.ProductCatalog.ReduceStock
{
	public record ReduceStockBulkCommand(List<ProductQuantityDto> Items)
	: IRequest<Response<List<ReducedProductDto>>>;
}
