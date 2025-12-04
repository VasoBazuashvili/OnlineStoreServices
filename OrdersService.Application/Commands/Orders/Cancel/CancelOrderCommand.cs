using MediatR;
using OrderService.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Commands.Orders.Cancel
{
	public record CancelOrderCommand(int OrderId, int UserId) : IRequest<Response<string>>;
}
