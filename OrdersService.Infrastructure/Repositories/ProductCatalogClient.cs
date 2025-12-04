using OrdersService.Application.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Orders.Infrastructure.External
{
	public class ProductCatalogClient : IProductCatalogClient
	{
		private readonly HttpClient _http;
		private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
		private const string DevServiceJwt = "dev-service-jwt";

		public ProductCatalogClient(HttpClient http)
		{
			_http = http;
		}

		private void SetAuthorizationHeader(HttpRequestMessage message, string? bearerToken)
		{
			message.Headers.Authorization =
				new AuthenticationHeaderValue("Bearer", string.IsNullOrEmpty(bearerToken) ? DevServiceJwt : bearerToken);
		}

		public async Task<IReadOnlyList<ReducedProductDto>> ReduceStockBulkAsync(
			IEnumerable<ProductQuantity> items,
			string? bearerToken = null,
			CancellationToken ct = default)
		{
			var reqBody = new { items = items.Select(i => new { productId = i.ProductId, quantity = i.Quantity }) };
			var message = new HttpRequestMessage(HttpMethod.Post, "api/products/reduce-bulk")
			{
				Content = new StringContent(JsonSerializer.Serialize(reqBody, _options), Encoding.UTF8, "application/json")
			};

			message.Headers.Authorization = new AuthenticationHeaderValue(
				"Bearer",
				string.IsNullOrEmpty(bearerToken) ? throw new InvalidOperationException("No service JWT provided") : bearerToken
			);

			var res = await _http.SendAsync(message, ct);
			if (!res.IsSuccessStatusCode)
			{
				var text = await res.Content.ReadAsStringAsync(ct);
				throw new HttpRequestException($"ReduceStockBulk failed ({res.StatusCode}): {text}");
			}

			var json = await res.Content.ReadAsStringAsync(ct);
			using var doc = JsonDocument.Parse(json);
			var arr = doc.RootElement.GetProperty("reducedProducts");

			var list = new List<ReducedProductDto>();
			foreach (var el in arr.EnumerateArray())
			{
				list.Add(new ReducedProductDto(
					el.GetProperty("productId").GetInt32(),
					el.GetProperty("name").GetString() ?? string.Empty,
					el.GetProperty("unitPrice").GetDecimal(),
					el.GetProperty("reducedBy").GetInt32(),
					el.GetProperty("remainingStock").GetInt32()
				));
			}

			return list;
		}

		public async Task IncreaseStockBulkAsync(
			IEnumerable<ProductQuantity> items,
			string? bearerToken = null,
			CancellationToken ct = default)
		{
			var reqBody = new { items = items.Select(i => new { productId = i.ProductId, quantity = i.Quantity }) };
			var message = new HttpRequestMessage(HttpMethod.Post, "api/products/increase-bulk")
			{
				Content = new StringContent(JsonSerializer.Serialize(reqBody, _options), Encoding.UTF8, "application/json")
			};

			SetAuthorizationHeader(message, bearerToken);

			var res = await _http.SendAsync(message, ct);
			if (!res.IsSuccessStatusCode)
			{
				var text = await res.Content.ReadAsStringAsync(ct);
				throw new HttpRequestException($"IncreaseStockBulk failed ({res.StatusCode}): {text}");
			}
		}

		public async Task<ProductDetailDto?> GetProductByIdAsync(
			int productId,
			string? bearerToken = null,
			CancellationToken ct = default)
		{
			var message = new HttpRequestMessage(HttpMethod.Get, $"api/products/{productId}");
			SetAuthorizationHeader(message, bearerToken);

			var res = await _http.SendAsync(message, ct);
			if (res.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

			res.EnsureSuccessStatusCode();
			var json = await res.Content.ReadAsStringAsync(ct);
			var root = JsonDocument.Parse(json).RootElement;

			return new ProductDetailDto(
				root.GetProperty("id").GetInt32(),
				root.GetProperty("name").GetString() ?? string.Empty,
				root.GetProperty("price").GetDecimal(),
				root.GetProperty("stockQuantity").GetInt32(),
				root.GetProperty("isActive").GetBoolean()
			);
		}
	}
}
