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

		public static Response<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
			=> new Response<T> { Data = data, StatusCode = statusCode };

		public static Response<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
			=> new Response<T> { ErrorMessage = errorMessage, StatusCode = statusCode };
	}

	public class Response
	{
		public string ErrorMessage { get; set; }
		public string Message { get; set; }
		public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
		public HttpStatusCode StatusCode { get; set; }

		public Response() { }

		public static Response Success(string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
	   => new Response { Message = message, StatusCode = statusCode };

		public static Response Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
			=> new Response { ErrorMessage = errorMessage, StatusCode = statusCode };
	}
}

