using MediatR;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Shared.DomainUtilities;
using ProductCatalogService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Queries.ProductCatalog.GetProducts
{
	public record GetProductsPagedQuery(int PageNumber, int PageSize) : IRequest<Response<PaginatedList<ProductDto>>>;
}
