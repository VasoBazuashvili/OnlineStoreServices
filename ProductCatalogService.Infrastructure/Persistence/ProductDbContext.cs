using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Infrastructure.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Infrastructure.Persistence
{
	public class ProductDbContext : DbContext
	{
		public ProductDbContext(DbContextOptions<ProductDbContext> options)
			: base(options) { }

		public DbSet<Product> Products { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{ 
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
		}	
	}
}
