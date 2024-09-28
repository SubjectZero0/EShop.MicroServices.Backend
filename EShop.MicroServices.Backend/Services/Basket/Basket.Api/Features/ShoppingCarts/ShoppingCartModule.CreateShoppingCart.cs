using Basket.Api.Features.ShoppingCarts.Commands.Create;
using Basket.Api.Features.ShoppingCarts.Queries.Search;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Basket.Api.Features.ShoppingCarts;

internal partial class ShoppingCartModule
{
	private void AddCreateShoppingCartEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/cart/create", async Task<IResult> ([FromBody] SearchShoppingCart request, ISender sender) =>
			{
				var existingCart = await sender.Send(request);

				if (existingCart is not null)
					return Results.Ok();

				await sender.Send(new CreateShoppingCart(request.UserName, request.CartId));

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