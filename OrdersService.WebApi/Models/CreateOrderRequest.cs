namespace OrdersService.WebApi.Models
{
	public class CreateOrderRequest
	{
		public List<CreateOrderRequestItem> Items { get; set; } = new();
	}

	public class CreateOrderRequestItem
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
	}
}
