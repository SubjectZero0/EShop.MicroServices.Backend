using Basket.Api.Constants;
using FluentValidation;

namespace Basket.Api.Features.ShoppingCarts.Queries.Search;

public class SearchShoppingCartValidator : AbstractValidator<SearchShoppingCart>
{
	public SearchShoppingCartValidator()
	{
		RuleFor(x => x.UserName)
			.NotNull().WithMessage("UserName cannot be null")
			.NotEmpty().WithMessage("UserName cannot be empty")
			.MaximumLength(255).WithMessage("UserName cannot exceed 255 characters");

		RuleFor(x => x.CartId)
			.NotEmpty().WithMessage("Id cannot be empty")
			.When(x => x.UserName != UserNames.DefaultUser)
			.NotNull().WithMessage("Id cannot be null")
			.When(x => x.UserName != UserNames.DefaultUser);
	}
}