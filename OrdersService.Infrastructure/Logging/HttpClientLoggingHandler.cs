using Microsoft.Extensions.Logging;

public class HttpClientLoggingHandler : DelegatingHandler
{
	private readonly ILogger<HttpClientLoggingHandler> _logger;

	public HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger)
	{
		_logger = logger;
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		_logger.LogInformation("Sending HTTP {Method} {Url}", request.Method, request.RequestUri);

		if (request.Content != null)
		{
			var body = await request.Content.ReadAsStringAsync(cancellationToken);
			_logger.LogDebug("Request Body: {Body}", body);
		}

		var response = await base.SendAsync(request, cancellationToken);

		_logger.LogInformation("Received {StatusCode} from {Url}", response.StatusCode, request.RequestUri);

		if (!response.IsSuccessStatusCode)
		{
			var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
			_logger.LogError("HTTP Error {Status}: {Body}", response.StatusCode, responseContent);
		}

		return response;
	}
}