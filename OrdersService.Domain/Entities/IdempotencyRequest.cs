using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Domain.Entities
{
	public class IdempotencyRequest
	{
		public int Id { get; set; }
		public string Key { get; set; } = null!; // Idempotency-Key header
		public string RequestHash { get; set; } = null!; // hash of payload+user
		public string ResponseData { get; set; } = null!; // JSON (order id/status)
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}
