using MediatR;
using OrderService.Shared.DomainUtilities;
using OrderService.Shared.Responses;
using OrdersService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Queries.GetOrders
{
	public record GetOrdersQuery(int UserId, int Page, int Size)
	: IRequest<Response<PaginatedList<OrderDto>>>;
}
