using MediatR;
using ProductCatalogService.Application.Commands.ProductCatalog.AddProduct;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Shared.Responses;

public class AddProductCommandHandler
	: IRequestHandler<AddProductCommand, Response<ProductDto>>
{
	private readonly IProductRepository _productRepository;
	private readonly IUnitOfWork _uow;

	public AddProductCommandHandler(IProductRepository productRepository, IUnitOfWork uow)
	{
		_productRepository = productRepository;
		_uow = uow;
	}

	public async Task<Response<ProductDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
	{
		var newProduct = new Product
		{
			Name = request.Name,
			SKU = request.SKU,
			Price = request.Price,
			StockQuantity = request.StockQuantity,
			IsActive = request.IsActive
		};

		await _productRepository.AddAsync(newProduct);
		await _uow.SaveChangesAsync(cancellationToken);



		var productDto = new ProductDto
		{
			Id = newProduct.Id,
			Name = newProduct.Name,
			SKU = newProduct.SKU,
			Price = newProduct.Price,
			StockQuantity = newProduct.StockQuantity,
			IsActive = newProduct.IsActive
		};

		return Response<ProductDto>.Success(productDto);
	}
}
