using MediatR;
using ProductCatalogService.Application.Commands.ProductCatalog.ReduceStock;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Shared.Responses;

public class ReduceStockBulkCommandHandler
	: IRequestHandler<ReduceStockBulkCommand, Response<List<ReducedProductDto>>>
{
	private readonly IProductRepository _repo;
	private readonly IUnitOfWork _uow;

	public ReduceStockBulkCommandHandler(
		IProductRepository repo,
		IUnitOfWork uow)
	{
		_repo = repo;
		_uow = uow;
	}

	public async Task<Response<List<ReducedProductDto>>> Handle(
		ReduceStockBulkCommand request,
		CancellationToken cancellationToken)
	{
		await _uow.BeginTransactionAsync(cancellationToken);

		try
		{
			var productIds = request.Items.Select(x => x.ProductId).ToList();
			var products = await _repo.GetByIdsAsync(productIds);

			if (products.Count != productIds.Count)
				return Response<List<ReducedProductDto>>
					.BadRequest("One or more products not found");

			foreach (var item in request.Items)
			{
				var product = products.Single(p => p.Id == item.ProductId);

				if (!product.IsActive)
					return Response<List<ReducedProductDto>>
						.BadRequest($"Product {product.Id} is inactive");

				if (product.StockQuantity < item.Quantity)
					return Response<List<ReducedProductDto>>
						.BadRequest($"Insufficient stock for product {product.Id}");
			}

			foreach (var item in request.Items)
			{
				var product = products.Single(p => p.Id == item.ProductId);
				product.StockQuantity -= item.Quantity;

				// BaseRepository.Update
				_repo.Update(product);
			}

			await _uow.SaveChangesAsync(cancellationToken);
			await _uow.CommitAsync(cancellationToken);

			var dtoList = products.Select(p =>
			{
				var qty = request.Items.Single(i => i.ProductId == p.Id).Quantity;

				return new ReducedProductDto
				{
					ProductId = p.Id,
					Name = p.Name,
					UnitPrice = p.Price,
					ReducedQuantity = qty
				};
			}).ToList();

			return Response<List<ReducedProductDto>>.Success(dtoList);
		}
		catch
		{
			await _uow.RollbackAsync(cancellationToken);
			throw;
		}
	}
}