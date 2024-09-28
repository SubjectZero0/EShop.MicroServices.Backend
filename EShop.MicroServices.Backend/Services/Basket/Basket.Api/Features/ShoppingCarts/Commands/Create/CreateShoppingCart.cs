using MediatR;
using Services.Shared.CQRS;

namespace Basket.Api.Features.ShoppingCarts.Commands.Create;

public record CreateShoppingCart(string UserName, Guid? Id = null) : ICommand<Unit>;