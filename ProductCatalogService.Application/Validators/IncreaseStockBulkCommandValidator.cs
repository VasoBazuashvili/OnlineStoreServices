using FluentValidation;
using ProductCatalogService.Application.Commands.ProductCatalog.IncreaseStock;

public class IncreaseStockBulkCommandValidator : AbstractValidator<IncreaseStockBulkCommand>
{
	public IncreaseStockBulkCommandValidator()
	{
		RuleFor(x => x.Items)
			.NotEmpty().WithMessage("Items list cannot be empty.");

		RuleForEach(x => x.Items).ChildRules(item =>
		{
			item.RuleFor(i => i.ProductId)
				.GreaterThan(0).WithMessage("ProductId must be greater than zero.");

			item.RuleFor(i => i.Quantity)
				.GreaterThan(0).WithMessage("Quantity must be greater than zero.");
		});
	}
}
