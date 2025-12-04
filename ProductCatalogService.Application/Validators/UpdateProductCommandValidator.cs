using FluentValidation;
using ProductCatalogService.Application.Commands.ProductCatalog.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
	public UpdateProductCommandValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0).WithMessage("Product Id must be greater than zero.");

		RuleFor(x => x.SKU)
			.NotEmpty().WithMessage("SKU is required.")
			.MaximumLength(20);

		RuleFor(x => x.Price)
			.GreaterThan(0).WithMessage("Price must be greater than zero.");

		RuleFor(x => x.StockQuantity)
			.GreaterThanOrEqualTo(0);

		RuleFor(x => x.IsActive)
			.NotNull();
	}
}