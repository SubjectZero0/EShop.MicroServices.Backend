using Basket.Api.Constants;
using FluentValidation;

namespace Basket.Api.Features.ShoppingCarts.Commands.Create;

public class CreateShoppingCartValidator : AbstractValidator<CreateShoppingCart>
{
    public CreateShoppingCartValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull().WithMessage("UserName cannot be null")
            .NotEmpty().WithMessage("UserName cannot be empty")
            .MaximumLength(255).WithMessage("UserName cannot exceed 255 characters");
        
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("If Shopping cart UserName is DefaultUser, Id it cannot be empty")
            .When(x => x.UserName == UserNames.DefaultUser)
            .NotNull().WithMessage("If Shopping cart UserName is DefaultUser, Id it cannot be null")
            .When(x => x.UserName == UserNames.DefaultUser);
    }
}