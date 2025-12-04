using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OrdersService.Application.Commands.Orders.Create;
using OrdersService.Application.Mapping;
using OrdersService.DependencyInjection;
using OrdersService.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication();
builder.Services.AddAppDependencies(configuration);

//builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Scheme = "Bearer",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Description = "JWT auth"
	});
	c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();