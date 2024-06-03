using Catalog.Api.Features.Products.Queries;
using MediatR;

namespace Catalog.Api.Features.Products
{
	internal partial class ProductModule
	{
		private void AddSearchProductsEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("/products/search", async (Guid? id, string? name, string[]? categories, decimal? price, ISender sender) =>
			{
				var query = new SearchProducts(
					Id: id,
					Name: name,
					Categories: categories,
					Price: price);

				var products = await sender.Send(query);

				return Results.Ok(products);
			})
			.WithName(nameof(SearchProducts))
			.Produces(200)
			.ProducesProblem(500)
			.WithOpenApi()
			.WithSummary("Search for products")
			.WithDescription("Provide the Product details in order to search for specific Products");
		}
	}
}