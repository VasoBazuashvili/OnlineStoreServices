using MediatR;
using Microsoft.Extensions.Configuration;
using OrderService.Shared.Responses;
using OrdersService.Application.Commands.Orders.Create;
using OrdersService.Application.DTOs;
using OrdersService.Application.Interfaces;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Enums;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public class CreateOrderCommandHandler
	: IRequestHandler<CreateOrderCommand, Response<CreateOrderResultDto>>
{
	private readonly IOrderRepository _ordersRepo;
	private readonly IProductCatalogClient _productClient;
	private readonly IIdempotencyRepository _idempotencyRepo;
	private readonly IUnitOfWork _uow;
	private readonly IConfiguration _configuration;
	private readonly IServiceTokenProvider _serviceTokenProvider;

	public CreateOrderCommandHandler(
		IOrderRepository ordersRepo,
		IProductCatalogClient productClient,
		IIdempotencyRepository idempotencyRepo,
		IUnitOfWork uow,
		IConfiguration configuration,
		IServiceTokenProvider serviceTokenProvider)
	{
		_ordersRepo = ordersRepo;
		_productClient = productClient;
		_idempotencyRepo = idempotencyRepo;
		_uow = uow;
		_configuration = configuration;
		_serviceTokenProvider = serviceTokenProvider;
	}

	public async Task<Response<CreateOrderResultDto>> Handle(
		CreateOrderCommand request,
		CancellationToken cancellationToken)
	{
		if (request.Items == null || !request.Items.Any())
			return Response<CreateOrderResultDto>.Fail("Order must contain items.");

		// 1) build request hash
		var requestHash = BuildRequestHash(request.UserId, request.Items);

		// 2) check idempotency
		var existing = await _idempotencyRepo.GetByKeyAsync(request.IdempotencyKey, cancellationToken);
		if (existing != null)
		{
			if (existing.RequestHash != requestHash)
				return Response<CreateOrderResultDto>.Fail("Idempotency key used with different payload.");

			var prev = JsonSerializer.Deserialize<CreateOrderResultDto>(existing.ResponseData)!;
			return Response<CreateOrderResultDto>.Success(prev);
		}

		// 3) call product catalog reduce-bulk
		var items = request.Items.Select(i => new ProductQuantity(i.ProductId, i.Quantity)).ToList();
		var token = _serviceTokenProvider.GenerateServiceToken();

		IReadOnlyList<ReducedProductDto> reduced;
		try
		{
			reduced = await _productClient.ReduceStockBulkAsync(items, token, cancellationToken);
		}
		catch (Exception ex)
		{
			return Response<CreateOrderResultDto>.Fail("Failed to reserve stock: " + ex.Message);
		}

		// 4) create order
		var order = new Order
		{
			UserId = request.UserId,
			Status = OrderStatus.Pending,
			CreatedAt = DateTime.Now
		};

		decimal total = 0m;
		foreach (var r in reduced)
		{
			var qty = request.Items.Single(x => x.ProductId == r.ProductId).Quantity;
			var item = new OrderItem
			{
				ProductId = r.ProductId,
				ProductName = r.Name,
				UnitPrice = r.UnitPrice,
				Quantity = qty
			};
			total += item.UnitPrice * item.Quantity;
			order.Items.Add(item);
		}
		order.TotalPrice = total;

		// 5) persist under unit of work
		try
		{
			await _uow.BeginTransactionAsync(cancellationToken);
			await _ordersRepo.AddAsync(order, cancellationToken);

			// store idempotency
			var resultDto = new CreateOrderResultDto(order.Id, order.Status);
			var idemp = new IdempotencyRequest
			{
				Key = request.IdempotencyKey,
				RequestHash = requestHash,
				ResponseData = JsonSerializer.Serialize(resultDto),
				CreatedAt = DateTime.Now
			};
			await _idempotencyRepo.AddAsync(idemp, cancellationToken);

			await _uow.CommitTransactionAsync(cancellationToken);
			return Response<CreateOrderResultDto>.Success(resultDto);
		}
		catch (Exception)
		{
			// rollback stock
			try
			{
				await _productClient.IncreaseStockBulkAsync(items, token, cancellationToken);
			}
			catch (Exception rollbackEx)
			{
				return Response<CreateOrderResultDto>.Fail(
					"Failed to persist order and rollback reservation failed: " + rollbackEx.Message,
					System.Net.HttpStatusCode.InternalServerError
				);
			}

			await _uow.RollbackTransactionAsync(cancellationToken);
			return Response<CreateOrderResultDto>.Fail(
				"Failed to create order.",
				System.Net.HttpStatusCode.InternalServerError
			);
		}
	}

	private static string BuildRequestHash(int userId, List<CreateOrderItemDto> items)
	{
		var arr = new
		{
			UserId = userId,
			Items = items.OrderBy(i => i.ProductId)
						 .Select(i => new { i.ProductId, i.Quantity })
		};
		var json = JsonSerializer.Serialize(arr);
		using var sha = SHA256.Create();
		var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
		return Convert.ToHexString(bytes);
	}
}
