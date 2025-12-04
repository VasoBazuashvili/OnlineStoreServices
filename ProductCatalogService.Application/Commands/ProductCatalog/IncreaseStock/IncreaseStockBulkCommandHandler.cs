using MediatR;
using ProductCatalogService.Application.Commands.ProductCatalog.IncreaseStock;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Shared.Responses;

public class IncreaseStockBulkCommandHandler
	: IRequestHandler<IncreaseStockBulkCommand, Response<string>>
{
	private readonly IProductRepository _repo;
	private readonly IUnitOfWork _uow;

	public IncreaseStockBulkCommandHandler(IProductRepository repo, IUnitOfWork uow)
	{
		_repo = repo;
		_uow = uow;
	}

	public async Task<Response<string>> Handle(
		IncreaseStockBulkCommand request,
		CancellationToken cancellationToken)
	{
		await _uow.BeginTransactionAsync(cancellationToken);

		try
		{
			var productIds = request.Items.Select(x => x.ProductId).ToList();
			var products = await _repo.GetByIdsAsync(productIds);

			if (products.Count != productIds.Count)
				return Response<string>.Fail("Some products not found.");

			foreach (var item in request.Items)
			{
				var product = products.Single(p => p.Id == item.ProductId);
				product.StockQuantity += item.Quantity;
			}

			await _uow.SaveChangesAsync(cancellationToken);
			await _uow.CommitAsync(cancellationToken);

			return Response<string>.Success("Stock increased successfully.");
		}
		catch
		{
			await _uow.RollbackAsync(cancellationToken);
			throw;
		}
	}
}