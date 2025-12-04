using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogService.WebApi.Middlewares
{
	internal sealed class GlobalExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<GlobalExceptionHandler> _logger;

		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) =>
			_logger = logger;

		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			_logger.LogError(exception,
							 "Exception occurred: {Message}",
							 exception.Message);

			var problemDetails = new ProblemDetails
			{
				Title = "An error occured",
				Detail = exception.Message,
				Type = exception.GetType().Name,
				Status = StatusCodes.Status500InternalServerError,
			};

			httpContext.Response.StatusCode = problemDetails.Status.Value;

			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

			return true;
		}
	}
}
