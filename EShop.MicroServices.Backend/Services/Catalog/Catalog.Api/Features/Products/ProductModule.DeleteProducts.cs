using Catalog.Api.Features.Products.Commands.Delete;
using MediatR;

namespace Catalog.Api.Features.Products
{
	internal partial class ProductModule
	{
		private void AddDeleteProductsEndpoint(IEndpointRouteBuilder app)
		{
			app.MapDelete("/products/delete", async Task<IResult> (Guid[] ids, ISender sender) =>
			{
				foreach (var id in ids)
				{
					var query = new DeleteProduct(Id: id);
					var products = await sender.Send(query);
				};

				return Results.NoContent();
			})
			.WithName("DeleteProducts")
			.Produces(204)
			.ProducesProblem(500)
			.WithOpenApi()
			.WithSummary("Delete a list of products")
			.WithDescription("Provide the ids of the Products for deletion");
		}
	}
}