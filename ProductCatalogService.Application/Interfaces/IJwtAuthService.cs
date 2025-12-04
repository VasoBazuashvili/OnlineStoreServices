using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Interfaces
{
	public interface IJwtAuthService
	{
		string GenerateToken(string userId, string role);
		ClaimsPrincipal? ValidateToken(string token);
	}
}
