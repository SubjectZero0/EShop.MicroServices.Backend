using Carter;
using MediatR;

namespace Basket.Api.Features.ShoppingCarts;

internal partial class ShoppingCartModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        AddCreateShoppingCartEndpoint(app);
    }   
}