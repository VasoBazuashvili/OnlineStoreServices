using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Application.Interfaces;
using ProductCatalogService.Infrastructure.Security;

namespace ProductCatalogService.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IJwtAuthService _jwtAuthService;

		public AuthController(IJwtAuthService jwtAuthService)
		{
			_jwtAuthService = jwtAuthService;
		}

		[HttpPost("token")]
		public IActionResult GetToken([FromQuery] string userId, [FromQuery] string role)
		{
			if (role != "Admin" && role != "User")
				return BadRequest("Invalid role");

			var token = _jwtAuthService.GenerateToken(userId, role);
			return Ok(new { Token = token });
		}

		[HttpGet("validate-token")]
		[Authorize]
		public IActionResult ValidateToken()
		{
			// Read the token from the Authorization header
			var authHeader = Request.Headers["Authorization"].ToString();
			if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
				return Unauthorized("Missing or invalid Authorization header");

			var token = authHeader.Substring("Bearer ".Length).Trim();

			var principal = _jwtAuthService.ValidateToken(token);
			if (principal == null)
				return Unauthorized("Invalid token");

			return Ok(new
			{
				UserId = principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value,
				Role = principal.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
			});
		}
	}
}
