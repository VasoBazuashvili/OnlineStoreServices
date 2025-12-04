using MediatR;
using OrderService.Shared.Responses;
using OrdersService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Queries.GetOrder
{
	public record GetOrderByIdQuery(int OrderId, int UserId) : IRequest<Response<OrderDto>?>;
}
