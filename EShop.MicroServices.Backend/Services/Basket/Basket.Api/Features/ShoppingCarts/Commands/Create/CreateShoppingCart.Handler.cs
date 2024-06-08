using Basket.Domain.Aggregates.ShoppingCarts;
using Marten;
using MediatR;
using Services.Shared.CQRS;

namespace Basket.Api.Features.ShoppingCarts.Commands.Create;

internal class CreateShoppingCartHandler : ICommandHandler<CreateShoppingCart, Unit>
{
    private readonly IDocumentSession _session;

    public CreateShoppingCartHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<Unit> Handle(CreateShoppingCart request, CancellationToken cancellationToken)
    {
        var newEmptyShoppingCart = ShoppingCart.CreateNew(
            userName: request.UserName,
            items: new List<ShoppingCartItem>(),
            timeStamp: DateTime.UtcNow);
        
        _session.Store(newEmptyShoppingCart);
        await _session.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}