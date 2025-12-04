using FluentValidation;
using OrdersService.Application.Commands.Orders.Cancel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Application.Validators
{
	public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
	{
		public CancelOrderCommandValidator()
		{
			RuleFor(x => x.OrderId)
				.GreaterThan(0).WithMessage("OrderId must be greater than zero.");

			RuleFor(x => x.UserId)
				.NotEmpty().WithMessage("UserId is required.");
		}
	}
}
