using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
		if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
			return BadRequest("UserId and Role are required.");

		var token = _jwtAuthService.GenerateToken(userId, role);
		return Ok(new { Token = token });
	}

	//ტესტირებისთვის
	[HttpGet("validate-token")]
	[Authorize]
	public IActionResult ValidateToken()
	{
		var authHeader = Request.Headers["Authorization"].ToString();
		if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
			return Unauthorized("Missing or invalid Authorization header");

		var token = authHeader.Substring("Bearer ".Length).Trim();
		var principal = _jwtAuthService.ValidateToken(token);
		if (principal == null)
			return Unauthorized("Invalid token");

		return Ok(new
		{
			UserId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
			Role = principal.FindFirst(ClaimTypes.Role)?.Value
		});
	}
}
