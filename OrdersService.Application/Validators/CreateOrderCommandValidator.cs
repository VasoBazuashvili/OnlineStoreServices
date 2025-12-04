using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using OrdersService.Application.Commands.Orders.Create;

namespace OrdersService.Application.Validators
{
	public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
	{
		public CreateOrderCommandValidator()
		{
			RuleFor(x => x.UserId)
				.NotEmpty().WithMessage("UserId is required.");

			RuleFor(x => x.Items)
				.NotEmpty().WithMessage("Order must contain at least one item.");

			RuleForEach(x => x.Items).ChildRules(item =>
			{
				item.RuleFor(i => i.ProductId)
					.GreaterThan(0).WithMessage("ProductId must be greater than zero.");

				item.RuleFor(i => i.Quantity)
					.GreaterThan(0).WithMessage("Quantity must be greater than zero.");
			});

			RuleFor(x => x.IdempotencyKey)
				.NotEmpty().WithMessage("Idempotency key is required.");
		}
	}
}
