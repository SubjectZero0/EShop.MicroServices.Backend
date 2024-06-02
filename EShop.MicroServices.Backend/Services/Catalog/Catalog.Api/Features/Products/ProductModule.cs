using Carter;
using MediatR;

namespace Catalog.Api.Features.Products
{
	internal partial class ProductModule : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			AddCreateProductEndpoint(app);
			AddSearchProductsEndpoint(app);
			AddUpdateProductEndpoint(app);
			AddDeleteProductEndpoint(app);
		}
	}
}