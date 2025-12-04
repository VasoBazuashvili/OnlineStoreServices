using FluentValidation;
using ProductCatalogService.Application.Commands.ProductCatalog.AddProduct;

public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
	public AddProductCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Product name is required.")
			.MaximumLength(50);

		RuleFor(x => x.SKU)
			.NotEmpty().WithMessage("SKU is required.")
			.MaximumLength(20);

		RuleFor(x => x.Price)
			.GreaterThan(0).WithMessage("Price must be greater than zero.");

		RuleFor(x => x.StockQuantity)
			.GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be zero or more.");
	}
}
