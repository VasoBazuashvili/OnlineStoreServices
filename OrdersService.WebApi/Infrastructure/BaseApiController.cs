using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Shared.Responses;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
	protected readonly IMediator _mediator;

	protected BaseApiController(IMediator mediator)
	{
		_mediator = mediator;
	}

	// Generic handler
	protected async Task<IActionResult> Handle<TResponse>(IRequest<Response<TResponse>> request)
	{
		var result = await _mediator.Send(request);
		if (result == null)
			return NotFound();

		if (!string.IsNullOrEmpty(result.ErrorMessage))
		{
			return result.StatusCode switch
			{
				System.Net.HttpStatusCode.BadRequest => BadRequest(result.ErrorMessage),
				System.Net.HttpStatusCode.NotFound => NotFound(result.ErrorMessage),
				_ => StatusCode((int)result.StatusCode, result.ErrorMessage)
			};
		}

		return Ok(result.Data);
	}

	// Helper to get userId from claims
	protected int? GetUserId()
	{
		var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
						User.FindFirstValue("sub");
		if (int.TryParse(userIdStr, out var userId))
			return userId;

		return null;
	}


	// Helper to get Idempotency-Key header
	protected string? GetIdempotencyKey() =>
		Request.Headers["Idempotency-Key"].FirstOrDefault();
}
