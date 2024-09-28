using Catalog.Api.Features.Products.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products
{
	internal partial class ProductModule
	{
		private void AddCreateProductEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("/products/create", async Task<IResult> ([FromBody] CreateProduct request, ISender sender) =>
			{
				await sender.Send(request);

				return Results.Created();
			})
			.WithName(nameof(CreateProduct))
			.Produces(201)
			.ProducesProblem(500)
			.WithOpenApi()
			.WithSummary("Create a new Product")
			.WithDescription("Provide the Product details in order to create a new Product");
		}
	}
}