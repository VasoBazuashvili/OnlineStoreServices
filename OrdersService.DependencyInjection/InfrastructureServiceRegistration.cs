using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Infrastructure.External;
using OrdersService.Application.Interfaces;
using OrdersService.Infrastructure.Persistence;
using OrdersService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.DependencyInjection
{
	public static class InfrastructureServiceRegistration
	{
		public static IServiceCollection AddInfrastructure(
			this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<OrdersDbContext>(opt =>
				opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			services.AddScoped<IOrderRepository, OrdersRepository>();
			services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddTransient<HttpClientLoggingHandler>();
			services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>(client =>
			{
				client.BaseAddress = new Uri(configuration["ProductCatalog:BaseUrl"]!.TrimEnd('/') + "/");
				client.Timeout = TimeSpan.FromSeconds(10);
			});

			return services;
		}
	}
}
