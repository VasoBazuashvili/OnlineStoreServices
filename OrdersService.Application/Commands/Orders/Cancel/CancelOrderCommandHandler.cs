using MediatR;
using Microsoft.Extensions.Configuration;
using OrderService.Shared.Responses;
using OrdersService.Application.Commands.Orders.Cancel;
using OrdersService.Application.Interfaces;
using OrdersService.Domain.Enums;
using System.Net;


public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Response<string>>
{
	private readonly IOrderRepository _ordersRepo;
	private readonly IProductCatalogClient _productClient;
	private readonly IUnitOfWork _uow;
	private readonly IConfiguration _configuration;

	public CancelOrderCommandHandler(
		IOrderRepository ordersRepo,
		IProductCatalogClient productClient,
		IUnitOfWork uow,
		IConfiguration configuration)
	{
		_ordersRepo = ordersRepo;
		_productClient = productClient;
		_uow = uow;
		_configuration = configuration;
	}

	public async Task<Response<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await _ordersRepo.GetByIdAsync(request.OrderId, cancellationToken);
		if (order == null)
			return Response<string>.Fail("Order not found", HttpStatusCode.NotFound);

		if (order.UserId != request.UserId)
			return Response<string>.Fail("Unauthorized", HttpStatusCode.Unauthorized);

		if (order.Status != OrderStatus.Pending)
			return Response<string>.Fail("Only Pending orders can be cancelled", HttpStatusCode.BadRequest);

		var items = order.Items.Select(i => new ProductQuantity(i.ProductId, i.Quantity)).ToList();

		var serviceJwt = _configuration["ProductCatalog:ServiceJwt"];
		if (string.IsNullOrEmpty(serviceJwt))
			return Response<string>.Fail("ProductCatalog service JWT is not configured.", HttpStatusCode.InternalServerError);

		await _uow.BeginTransactionAsync(cancellationToken);
		try
		{
			await _productClient.IncreaseStockBulkAsync(items, serviceJwt, cancellationToken);

			order.Status = OrderStatus.Cancelled;
			await _ordersRepo.UpdateAsync(order, cancellationToken);

			await _uow.CommitTransactionAsync(cancellationToken);
			return Response<string>.Success("Order cancelled successfully");
		}
		catch (Exception ex)
		{
			await _uow.RollbackTransactionAsync(cancellationToken);
			return Response<string>.Fail("Failed to cancel order: " + ex.Message, HttpStatusCode.InternalServerError);
		}
	}
}