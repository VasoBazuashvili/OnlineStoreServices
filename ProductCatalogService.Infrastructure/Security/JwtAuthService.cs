using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductCatalogService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ProductCatalogService.Infrastructure.Security.JwtAuthService;

namespace ProductCatalogService.Infrastructure.Security
{
	public class JwtAuthService : IJwtAuthService
	{
		private readonly string _secretKey;
		private readonly string _issuer;
		private readonly string _audience;

		public JwtAuthService(IConfiguration configuration)
		{
			_secretKey = configuration["Jwt:Key"]!;
			_issuer = configuration["Jwt:Issuer"]!;
			_audience = configuration["Jwt:Audience"]!;
		}

		public string GenerateToken(string userId, string role)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, userId),
				new Claim(ClaimTypes.Role, role),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
			var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

			var now = DateTimeOffset.UtcNow;
			var validUntil = now.AddHours(1);
			long unixExp = validUntil.ToUnixTimeSeconds();

			var expiresDateTime = DateTimeOffset.FromUnixTimeSeconds(unixExp).UtcDateTime;

			var token = new JwtSecurityToken(
				issuer: _issuer,
				audience: _audience,
				claims: claims,
				notBefore: now.UtcDateTime,
				expires: expiresDateTime,
				signingCredentials: creds
			);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public ClaimsPrincipal? ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_secretKey);
			try
			{
				var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = _issuer,
					ValidateAudience = false,
					ValidAudience = _audience,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				return principal;
			}
			catch
			{
				return null;
			}
		}
	}
}
