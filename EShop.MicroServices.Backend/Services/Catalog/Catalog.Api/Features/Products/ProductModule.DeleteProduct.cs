using Catalog.Api.Features.Products.Commands;
using MediatR;

namespace Catalog.Api.Features.Products
{
	internal partial class ProductModule
	{
		private void AddDeleteProductEndpoint(IEndpointRouteBuilder app)
		{
			app.MapDelete("/products/delete/{id}", async (Guid id, ISender sender) =>
			{
				var query = new DeleteProduct(Id: id);

				var products = await sender.Send(query);

				return Results.NoContent();
			})
			.WithName(nameof(DeleteProduct))
			.Produces(204)
			.ProducesProblem(500)
			.WithOpenApi()
			.WithSummary("Delete a product")
			.WithDescription("Provide the id of the Product for deletion");
		}
	}
}