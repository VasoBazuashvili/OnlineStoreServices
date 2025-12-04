using MediatR;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Commands.ProductCatalog.UpdateProduct
{
	public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Response<Unit>>
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _uow;
		public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork uow)
		{
			_productRepository = productRepository;
			_uow = uow;
		}

		public async Task<Response<Unit>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var existingProduct = await _productRepository.GetByIdAsync(request.Id);
			if (existingProduct == null)
				return Response<Unit>.Fail("Product not found");

			existingProduct.SKU = request.SKU;
			existingProduct.Price = request.Price;
			existingProduct.StockQuantity = request.StockQuantity;
			existingProduct.IsActive = request.IsActive;

			_productRepository.Update(existingProduct);

			await _uow.SaveChangesAsync(cancellationToken);

			return Response<Unit>.Success(Unit.Value);
		}
	}
}
