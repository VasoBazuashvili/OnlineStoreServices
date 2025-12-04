using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrdersService.Application.Interfaces;
using OrdersService.Infrastructure.Security;
using System.Text;

public static class DependencyResolver
{
	public static IServiceCollection AddAppDependencies(
		this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IJwtAuthService, JwtAuthService>();
		services.AddSingleton<IServiceTokenProvider,ServiceTokenProvider>();

		var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidIssuer = configuration["Jwt:Issuer"],
				ValidateAudience = true,
				ValidAudience = configuration["Jwt:Audience"],
				ClockSkew = TimeSpan.Zero
			};
		});
		services.AddAuthorization();

		services.AddHttpContextAccessor();

		return services;
	}
}