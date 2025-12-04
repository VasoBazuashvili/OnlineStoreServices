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
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> b)
		{
			b.HasKey(x => x.Id);

			b.Property(x => x.TotalPrice)
				.HasPrecision(18, 2);

			b.HasMany(x => x.Items)
				.WithOne()
				.HasForeignKey(x => x.OrderId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
