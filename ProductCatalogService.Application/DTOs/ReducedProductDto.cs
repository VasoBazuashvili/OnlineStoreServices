using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.DTOs
{
	public class ReducedProductDto
	{
		public int ProductId { get; set; }
		public string Name { get; set; } = string.Empty;
		public decimal UnitPrice { get; set; }
		public int ReducedQuantity { get; set; }
	}
}
