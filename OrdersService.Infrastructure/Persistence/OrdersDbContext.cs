using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Infrastructure.Persistence
{
	public class OrdersDbContext : DbContext
	{
		public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

		public DbSet<Order> Orders => Set<Order>();
		public DbSet<OrderItem> OrderItems => Set<OrderItem>();
		public DbSet<IdempotencyRequest> IdempotencyRequests => Set<IdempotencyRequest>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);
		}
	}
}
