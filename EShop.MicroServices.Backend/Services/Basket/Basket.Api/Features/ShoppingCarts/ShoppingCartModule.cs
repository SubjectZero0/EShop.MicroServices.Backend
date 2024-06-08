using Carter;
using MediatR;

namespace Basket.Api.Features.ShoppingCarts;

internal partial class ShoppingCartModule : ICarterModule
{
    private readonly ISender _sender;

    public ShoppingCartModule(ISender sender)
    {
        _sender = sender;
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        AddCreateShoppingCartEndpoint(app);
    }   
}