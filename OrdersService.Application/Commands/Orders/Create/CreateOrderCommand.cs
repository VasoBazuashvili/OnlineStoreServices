using MediatR;
using OrderService.Shared.Responses;
using OrdersService.Application.DTOs;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Commands.Orders.Create
{
	public record CreateOrderCommand(
	int UserId,
	List<CreateOrderItemDto> Items,
	string IdempotencyKey
	) : IRequest<Response<CreateOrderResultDto>>;
}
