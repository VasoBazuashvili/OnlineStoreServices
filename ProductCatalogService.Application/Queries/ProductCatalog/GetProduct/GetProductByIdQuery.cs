using MediatR;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Queries.ProductCatalog.GetProduct
{
	public record GetProductByIdQuery(int Id) : IRequest<Response<ProductDto>>;
}
