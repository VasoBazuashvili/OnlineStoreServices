using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Application.Commands;
using OrdersService.Application.Commands.Orders.Cancel;
using OrdersService.Application.Commands.Orders.Create;
using OrdersService.Application.DTOs;
using OrdersService.Application.Queries;
using OrdersService.Application.Queries.GetOrder;
using OrdersService.Application.Queries.GetOrders;
using OrdersService.WebApi.Models;
using System.Security.Claims;

namespace OrdersService.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class OrdersController : BaseApiController
	{
		public OrdersController(IMediator mediator) : base(mediator) { }

		[HttpPost]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> Create([FromHeader(Name = "Idempotency-Key")] string idempotencyKey, [FromBody] CreateOrderRequest request)
		{
			//var idempotencyKey = GetIdempotencyKey();
			if (string.IsNullOrEmpty(idempotencyKey))
				return BadRequest("Idempotency-Key header required.");

			var userId = GetUserId();
			if (userId == null) return Unauthorized();

			var items = request.Items.Select(x => new CreateOrderItemDto(x.ProductId, x.Quantity)).ToList();
			var cmd = new CreateOrderCommand(userId.Value, items, idempotencyKey);
			return await Handle(cmd);
		}

		[HttpGet("{id}")]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> GetById(int id)
		{
			var userId = GetUserId();
			if (userId == null) return Unauthorized();

			var qr = new GetOrderByIdQuery(id, userId.Value);
			return await Handle(qr);
		}

		[HttpGet]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int size = 10)
		{
			var userId = GetUserId();
			if (userId == null) return Unauthorized();

			var qr = new GetOrdersQuery(userId.Value, page, size);
			return await Handle(qr);
		}

		[HttpPost("{id}/cancel")]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> Cancel(int id)
		{
			var userId = GetUserId();
			if (userId == null) return Unauthorized();

			var cmd = new CancelOrderCommand(id, userId.Value);
			return await Handle(cmd);
		}
	}
}
