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
		public string Key { get; set; } = null!;
		public string RequestHash { get; set; } = null!;
		public string ResponseData { get; set; } = null!;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}
