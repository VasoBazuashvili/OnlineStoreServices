using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Infrastructure.Persistence.Configuration
{
	public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> b)
		{
			b.HasKey(x => x.Id);

			b.Property(x => x.UnitPrice)
				.HasPrecision(18, 2);

			b.Property(x => x.ProductName)
				.HasMaxLength(200);

			b.Property(x => x.ProductId)
				.HasMaxLength(50);
		}
	}
}
