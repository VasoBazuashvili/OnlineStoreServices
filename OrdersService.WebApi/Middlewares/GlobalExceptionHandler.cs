using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OrdersService.WebApi.Middlewares
{
	internal sealed class GlobalExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<GlobalExceptionHandler> _logger;

		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) =>
			_logger = logger;

		public async ValueTask<bool> TryHandleAsync(
			HttpContext httpContext,
			Exception exception,
			CancellationToken cancellationToken)
		{
			_logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

			var problem = new ProblemDetails
			{
				Title = "Unexpected server error",
				Detail = exception.Message,
				Type = exception.GetType().Name,
				Status = StatusCodes.Status500InternalServerError
			};

			httpContext.Response.StatusCode = problem.Status.Value;
			await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

			return true;
		}
	}
}
