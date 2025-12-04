using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Shared.Responses
{
	public class Response<T>
	{
		public T Data { get; set; }
		public string ErrorMessage { get; set; }
		public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
		public HttpStatusCode StatusCode { get; set; }

		public Response() { }

		// Success factory
		public static Response<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
			=> new Response<T> { Data = data, StatusCode = statusCode };

		// Failure factory
		public static Response<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
			=> new Response<T> { ErrorMessage = errorMessage, StatusCode = statusCode };
	}

	// Non-generic response for commands that don't return data
	public class Response
	{
		public string ErrorMessage { get; set; }
		public string Message { get; set; }
		public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
		public HttpStatusCode StatusCode { get; set; }

		public Response() { }

		// Success factory
		public static Response Success(string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
	   => new Response { Message = message, StatusCode = statusCode };

		// Failure factory
		public static Response Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
			=> new Response { ErrorMessage = errorMessage, StatusCode = statusCode };
	}
}

