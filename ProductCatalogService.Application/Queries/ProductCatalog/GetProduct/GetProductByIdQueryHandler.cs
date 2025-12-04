using MediatR;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Queries.ProductCatalog.GetProduct
{
	public class GetProductByIdQueryHandler
	: IRequestHandler<GetProductByIdQuery, Response<ProductDto>>
	{
		private readonly IProductRepository _productRepository;

		public GetProductByIdQueryHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Response<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);
			if (product == null)
			{
				return Response<ProductDto>.Fail($"Product with ID {request.Id} not found.");
			}

			var dto = new ProductDto
			{
				Id = product.Id,
				Name = product.Name,
				SKU = product.SKU,
				Price = product.Price,
				StockQuantity = product.StockQuantity
			};
			return Response<ProductDto>.Success(dto);
		}
	}
}
