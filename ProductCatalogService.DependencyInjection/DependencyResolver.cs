using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogService.Application.Commands.ProductCatalog.AddProduct;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Infrastructure.Persistence;
using ProductCatalogService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.DependencyInjection
{
	public class DependencyResolver
	{
		private readonly IConfiguration _configuration;

		public DependencyResolver(IConfiguration configuration)
			=> _configuration = configuration;

		public IServiceCollection Resolve(IServiceCollection services)
		{
			// Database
			var connectionString = _configuration.GetConnectionString(nameof(ProductDbContext));
			services.AddDbContext<ProductDbContext>(options =>
				options.UseSqlServer(connectionString));

			// Repositories
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			// Application layer
			services.AddApplications(typeof(AddProductCommand).Assembly);

			// Fluent Validation (if not already added by AddApplications)
			services.AddFluentValidationAutoValidation();
			services.AddFluentValidationClientsideAdapters();

			return services;
		}
	}
}
