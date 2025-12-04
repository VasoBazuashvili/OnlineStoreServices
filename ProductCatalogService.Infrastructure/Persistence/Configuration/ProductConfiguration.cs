using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Infrastructure.Persistence.Configuration
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> entity)
		{
			entity.Property(e => e.Name)
			  .IsRequired()
			  .HasMaxLength(50);

			entity.Property(e => e.SKU)
				  .IsRequired()
				  .HasMaxLength(20);

			entity.Property(e => e.Price)
				  .IsRequired()
				  .HasPrecision(18, 2);

			entity.Property(e => e.StockQuantity)
				  .IsRequired();

			entity.Property(e => e.IsActive)
				  .IsRequired();
		}
	}
}
