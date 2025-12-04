using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.DTOs
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string SKU { get; set; } = null!;
		public decimal Price { get; set; }
		public int StockQuantity { get; set; }
		public bool IsActive { get; set; }
	}
}
