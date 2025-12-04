using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Shared.Responses
{
	public class Response<T>
	{
		public T Data { get; set; }
		public string ErrorMessage { get; set; }
		public HttpStatusCode StatusCode { get; set; }

		public static Response<T> Success(T data)
			=> new Response<T> { Data = data, StatusCode = HttpStatusCode.OK };

		public static Response<T> NotFound(string message)
			=> new Response<T> { StatusCode = HttpStatusCode.NotFound, ErrorMessage = message };

		public static Response<T> BadRequest(string message)
			=> new Response<T> { StatusCode = HttpStatusCode.BadRequest, ErrorMessage = message };

		public static Response<T> Fail(string message)
			=> new Response<T> { StatusCode = HttpStatusCode.InternalServerError, ErrorMessage = message };
	}
}
