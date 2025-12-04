using AutoMapper;
using MediatR;
using OrderService.Shared.Responses;
using OrdersService.Application.DTOs;
using OrdersService.Application.Interfaces;
using OrdersService.Application.Queries.GetOrder;
using System.Net;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Response<OrderDto>?>
{
    private readonly IOrderRepository _ordersRepo;
	private readonly IMapper _mapper;
	public GetOrderByIdQueryHandler(IOrderRepository ordersRepo, IMapper mapper)
	{
		_ordersRepo = ordersRepo;
		_mapper = mapper;
	}

	public async Task<Response<OrderDto>?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
	{
		var order = await _ordersRepo.GetByIdAsync(request.OrderId, cancellationToken);
		if (order == null)
			return Response<OrderDto>.Fail($"Order with ID {request.OrderId} not found.");

		if (order.UserId != request.UserId)
			return Response<OrderDto>.Fail("Unauthorized access to this order.", HttpStatusCode.Unauthorized);

		var dto = _mapper.Map<OrderDto>(order);
		return Response<OrderDto>.Success(dto);
	}
}
