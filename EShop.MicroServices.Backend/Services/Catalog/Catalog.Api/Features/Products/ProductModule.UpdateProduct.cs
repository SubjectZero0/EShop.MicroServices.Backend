using Catalog.Api.Features.Products.Commands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products
{
	internal partial class ProductModule
	{
		private void AddUpdateProductEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("/products/update", async ([FromBody] UpdateProduct request, ISender sender) =>
			{
				await sender.Send(request);

				return Results.Ok();
			})
			.WithName(nameof(UpdateProduct))
			.Produces(200)
			.ProducesProblem(500)
			.WithOpenApi()
			.WithSummary("Update an existing Product")
			.WithDescription("Provide the Product details in order to update an existing Product based on its Id.");
		}
	}
}