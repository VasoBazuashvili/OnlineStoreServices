using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Shared.Responses;
using System.Net;

namespace ProductCatalogService.WebApi.Infrastructure
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class BaseApiController : ControllerBase
	{
		private readonly IMediator _mediator;

		protected BaseApiController(IMediator mediator)
		{
			_mediator = mediator;
		}

		protected async Task<IActionResult> Handle<TResponse>(IRequest<Response<TResponse>> request)
		{
			var response = await _mediator.Send(request);

			switch (response.StatusCode)
			{
				case HttpStatusCode.NotFound:
					return NotFound(response);

				default: return Ok(response);
			}
		}
	}
}
