using Basket.Api.Features.ShoppingCarts.Commands.Create;
using Basket.Api.Features.ShoppingCarts.Queries.Search;
using MediatR;
using Services.Shared.Retrievals;

namespace Basket.Api.Features.ShoppingCarts;

internal partial class ShoppingCartModule
{
    private void AddCreateShoppingCartEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/cart/create", async (SearchShoppingCart request, ISender sender) =>
            {
                //TODO: Implement retrieval
                var existingCart = await _sender.Send(request);
                
                if (existingCart is not null)
                    await sender.Send(new CreateShoppingCart(request.UserName));

                return Results.Created();
            })
            .WithName("CreateShoppingCart")
            .Produces(201)
            .ProducesProblem(500)
            .WithOpenApi()
            .WithSummary("Create a new Shopping Cart if one is not found for the specified user name.");
    }

}