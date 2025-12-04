using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace ProductCatalogService.DependencyInjection
{
	public static class ApplicationResolver
	{
		public static IServiceCollection AddApplications(this IServiceCollection services, params Assembly[] assemblies)
		{
			foreach (var assembly in assemblies)
			{
				services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
				services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
				services.AddValidatorsFromAssembly(assembly);
			}

			return services;
		}
	}
}
