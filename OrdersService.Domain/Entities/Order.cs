using OrdersService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Domain.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public decimal TotalPrice { get; set; }
		public OrderStatus Status { get; set; }
		public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
	}
}
