using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Domain.Entities
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }        // max 50 chars
		public string SKU { get; set; }         // max 20 chars
		public decimal Price { get; set; }      // positive
		public int StockQuantity { get; set; }
		public bool IsActive { get; set; }
	}
}
