using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Application.Commands.ProductCatalog.AddProduct;
using ProductCatalogService.Application.Commands.ProductCatalog.IncreaseStock;
using ProductCatalogService.Application.Commands.ProductCatalog.ReduceStock;
using ProductCatalogService.Application.Commands.ProductCatalog.UpdateProduct;
using ProductCatalogService.Application.DTOs;
using ProductCatalogService.Application.Queries;
using ProductCatalogService.Application.Queries.ProductCatalog.GetProduct;
using ProductCatalogService.Application.Queries.ProductCatalog.GetProducts;
using ProductCatalogService.WebApi.Infrastructure;

namespace ProductCatalogService.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : BaseApiController
	{
		public ProductsController(IMediator mediator) : base(mediator) { }
		// -------------------------------------------------------------
		// GET: /api/products?pageNumber=1&pageSize=10
		// -------------------------------------------------------------
		[HttpGet]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
			=> await Handle(new GetProductsPagedQuery(pageNumber, pageSize));

		// -------------------------------------------------------------
		// GET: /api/products/{id}
		// -------------------------------------------------------------
		[HttpGet("{id}")]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> GetProduct(int id)
		=> await Handle(new GetProductByIdQuery(id));

		// -------------------------------------------------------------
		// POST: /api/products
		// -------------------------------------------------------------
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
			=> await Handle(new AddProductCommand(command.Name, command.SKU, command.Price, command.StockQuantity, command.IsActive));

		// -------------------------------------------------------------
		// PUT: /api/products/{id}
		// -------------------------------------------------------------
		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
			=> await Handle(new UpdateProductCommand(id, command.SKU, command.Price, command.StockQuantity, command.IsActive));

		// -------------------------------------------------------------
		// POST: /api/products/{id}/reduce-stock?quantity=5
		// -------------------------------------------------------------
		[HttpPost("reduce-bulk")]
		[Authorize(Roles = "User, Admin")]
		public Task<IActionResult> ReduceStockBulk([FromBody] ReduceStockBulkCommand command)
			=> Handle(command);


		// -------------------------------------------------------------
		// POST: /api/products/increase-bulk
		// -------------------------------------------------------------

		[HttpPost("increase-bulk")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> IncreaseBulk([FromBody] IncreaseStockBulkCommand command)
			=> await Handle(command);
	}
}

