using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrdersService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Infrastructure.Security
{
	public class JwtAuthService : IJwtAuthService
	{
		private readonly string _secretKey;
		private readonly string _issuer;
		private readonly string _audience;

		public JwtAuthService(IConfiguration configuration)
		{
			_secretKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
			_issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
			_audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");
		}
		public string GenerateToken(string userId, string role, TimeSpan? expiresIn = null)
		{
			var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, userId),
			new Claim(ClaimTypes.Role, role),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
			var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
			// 1. Current time in UTC
			var now = DateTimeOffset.UtcNow;

			// 2. Add desired validity period, e.g., 5 minutes
			var validUntil = now.AddMinutes(15);

			// 3. Convert to Unix timestamp (seconds since 1970-01-01 UTC)
			long unixExp = validUntil.ToUnixTimeSeconds();

			// 4. If needed, convert back to DateTime for JwtSecurityToken
			var expiresDateTime = DateTimeOffset.FromUnixTimeSeconds(unixExp).UtcDateTime;

			// 5. Use in JWT token
			var token = new JwtSecurityToken(
				issuer: _issuer,
				audience: _audience,
				claims: claims,
				notBefore: now.UtcDateTime,
				expires: expiresDateTime,      // will expire in 5 minutes
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_secretKey);

			try
			{
				var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = validateLifetime,
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
