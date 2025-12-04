using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductCatalogService.Application.Commands;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.DependencyInjection;
using ProductCatalogService.Infrastructure.Persistence;
using ProductCatalogService.Infrastructure.Repositories;
using ProductCatalogService.Infrastructure.Security;
using ProductCatalogService.WebApi.Middlewares;
using System.Runtime.Loader;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

new DependencyResolver(builder.Configuration).Resolve(builder.Services);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "ProductCatalog API",
		Version = "v1"
	});

	// JWT Bearer in Swagger
	c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. Use 'dev-service-jwt' for internal calls."
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
			Array.Empty<string>()
		}
	});
});

var app = builder.Build();

// ---------------------------
// Middleware
// ---------------------------
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ Correct order: Authentication first, then Authorization
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

app.Run();