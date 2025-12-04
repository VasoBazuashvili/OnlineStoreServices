using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrdersService.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class ServiceTokenProvider : IServiceTokenProvider
{
	private readonly IConfiguration _configuration;
	public ServiceTokenProvider(IConfiguration configuration) => _configuration = configuration;

	public string GenerateServiceToken()
	{
		var key = Encoding.UTF8.GetBytes(_configuration["ServiceJwt:Key"]);
		var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, "OrdersService"),
			new Claim(ClaimTypes.Role, "Admin")
		};

		var token = new JwtSecurityToken(
			issuer: _configuration["ServiceJwt:Issuer"],
			audience: _configuration["ServiceJwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["ServiceJwt:ExpiryMinutes"])),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
