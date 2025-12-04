using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.DependencyInjection
{
	public static class InfrastructureResolver
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			// JWT Auth service
			services.AddSingleton<IJwtAuthService, JwtAuthService>();

			var jwtSettings = configuration.GetSection("Jwt");
			var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

			// Authentication
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = jwtSettings["Issuer"],
					ValidateAudience = true,
					ValidAudience = jwtSettings["Audience"],
					ClockSkew = TimeSpan.Zero
				};

				// Dev token for internal service calls
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var token = context.Request.Headers["Authorization"]
										   .ToString().Replace("Bearer ", "");
						if (token == "dev-service-jwt")
						{
							// bypass validation for dev/demo
							var claims = new[]
							{
							new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "service"),
							new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, "OrderService")
							};
							context.Principal = new System.Security.Claims.ClaimsPrincipal(
								new System.Security.Claims.ClaimsIdentity(claims, "dev"));
							context.Success();
						}
						return Task.CompletedTask;
					}
				};
			});

			services.AddAuthorization();

			return services;
		}
	}
}
