using Basket.Api.Features.ShoppingCarts.Commands.Create;
using Basket.Api.Features.ShoppingCarts.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Features.ShoppingCarts;

internal partial class ShoppingCartModule
{
    private static void AddCreateShoppingCartEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/cart/create", async ([FromBody] SearchShoppingCart request, ISender sender) =>
            {
                var existingCart = await sender.Send(request);

                if (existingCart is not null) 
                    return Results.Ok();
                
                await sender.Send(new CreateShoppingCart(request.UserName, request.Id));
                return Results.Created();
            })
            .WithName("CreateShoppingCart")
            .Produces(201)
            .Produces(200)
            .ProducesProblem(500)
            .WithOpenApi()
            .WithSummary("Create a new Shopping Cart if one is not found for the specified user name/Id.");
    }

}