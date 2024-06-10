using Basket.Domain.Aggregates.ShoppingCarts;
using MediatR;
using Services.Shared.CQRS;
using Services.Shared.Storage;

namespace Basket.Api.Features.ShoppingCarts.Commands.Create;

internal class CreateShoppingCartHandler : ICommandHandler<CreateShoppingCart, Unit>
{
    private readonly IStorage<ShoppingCart> _storage;

    public CreateShoppingCartHandler(IStorage<ShoppingCart> storage)
    {
        _storage = storage;
    }

    public async Task<Unit> Handle(CreateShoppingCart request, CancellationToken cancellationToken)
    {
        var newEmptyShoppingCart = ShoppingCart.CreateNew(
            id: request.Id ?? Guid.NewGuid(),
            userName: request.UserName,
            items: new List<ShoppingCartItem>(),
            timeStamp: DateTime.UtcNow);

        await _storage.Store(newEmptyShoppingCart);

        return Unit.Value;
    }
}