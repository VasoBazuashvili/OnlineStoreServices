using AutoMapper;
using MediatR;
using OrderService.Shared.DomainUtilities;
using OrderService.Shared.Responses;
using OrdersService.Application.DTOs;
using OrdersService.Application.Interfaces;
using OrdersService.Application.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Response<PaginatedList<OrderDto>>>
{
	private readonly IOrderRepository _ordersRepo;
	private readonly IMapper _mapper;

	public GetOrdersQueryHandler(IOrderRepository ordersRepo, IMapper mapper)
	{
		_ordersRepo = ordersRepo;
		_mapper = mapper;
	}

	public async Task<Response<PaginatedList<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
	{
		// Retrieve orders for user with pagination
		var orders = await _ordersRepo.GetByUserAsync(request.UserId, request.Page, request.Size, cancellationToken);
		var totalCount = await _ordersRepo.CountByUserAsync(request.UserId, cancellationToken);

		if (!orders.Any())
			return Response<PaginatedList<OrderDto>>.Success(new PaginatedList<OrderDto>(new List<OrderDto>(), request.Page, request.Size, 0));

		// Map entities to DTOs
		var orderDtos = orders.Select(o => _mapper.Map<OrderDto>(o)).ToList();

		var result = new PaginatedList<OrderDto>(orderDtos, request.Page, request.Size, totalCount);

		return Response<PaginatedList<OrderDto>>.Success(result);
	}
}