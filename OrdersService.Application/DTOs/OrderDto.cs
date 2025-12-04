using OrdersService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.DTOs
{
	public class OrderDto
	{
		public int Id { get; set; }
		public Guid UserId { get; set; }
		public DateTime CreatedAt { get; set; }
		public decimal TotalPrice { get; set; }
		public OrderStatus Status { get; set; }
		public List<OrderItemDto> Items { get; set; } = new();
	}
}
