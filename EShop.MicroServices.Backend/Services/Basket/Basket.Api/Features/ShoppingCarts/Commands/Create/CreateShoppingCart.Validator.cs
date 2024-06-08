using FluentValidation;

namespace Basket.Api.Features.ShoppingCarts.Commands.Create;

public class CreateShoppingCartValidator : AbstractValidator<CreateShoppingCart>
{
    public CreateShoppingCartValidator()
    {
    }
}