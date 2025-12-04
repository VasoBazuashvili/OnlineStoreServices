using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.Application.Commands.Orders.Create;
using OrdersService.Application.Mapping;

public static class ApplicationResolver
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

		services.AddAutoMapper(cfg => { }, typeof(OrderMappingProfile).Assembly);

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

		services.AddValidatorsFromAssembly(typeof(OrdersService.Application.Validators.CreateOrderCommandValidator).Assembly);

		return services;
	}
}
