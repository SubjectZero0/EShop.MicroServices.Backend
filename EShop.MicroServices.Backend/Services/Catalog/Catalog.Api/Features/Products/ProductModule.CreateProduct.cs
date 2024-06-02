using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products
{
	internal partial class ProductModule : ICarterModule
	{
		private void AddCreateProductEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("/products/create", async ([FromBody] CreateProduct request, ISender sender) =>
			{
				await sender.Send(new CreateProduct(
					Name: request.Name,
					Description: request.Description,
					ImageFile: request.ImageFile,
					Price: request.Price,
					Categories: request.Categories));

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