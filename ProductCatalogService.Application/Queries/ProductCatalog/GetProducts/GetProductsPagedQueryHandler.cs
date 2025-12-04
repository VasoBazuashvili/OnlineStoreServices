using MediatR;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Shared.DomainUtilities;
using ProductCatalogService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Queries.ProductCatalog.GetProducts
{
	public class GetProductsPagedQueryHandler : IRequestHandler<GetProductsPagedQuery, Response<PaginatedList<ProductDto>>>
	{
		private readonly IProductRepository _productRepository;

		public GetProductsPagedQueryHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Response<PaginatedList<ProductDto>>> Handle(GetProductsPagedQuery request, CancellationToken cancellationToken)
		{
			var products = await _productRepository.GetProductsAsync(request.PageNumber, request.PageSize);
			var totalCount = await _productRepository.CountAsync();

			var productDtos = products.Select(p => new ProductDto
			{
				Id = p.Id,
				Name = p.Name,
				SKU = p.SKU,
				Price = p.Price,
				StockQuantity = p.StockQuantity
			});

			var pagedList = new PaginatedList<ProductDto>(
				productDtos,
				totalCount,
				request.PageNumber,
				request.PageSize
			);

			return Response<PaginatedList<ProductDto>>.Success(pagedList);
		}
	}
}
