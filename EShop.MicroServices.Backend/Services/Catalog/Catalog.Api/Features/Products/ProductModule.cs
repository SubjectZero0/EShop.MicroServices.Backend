using Carter;
using MediatR;

namespace Catalog.Api.Features.Products
{
	internal partial class ProductModule : ICarterModule
	{
		private readonly IMediator _mediator;

		public ProductModule(IMediator mediator)
		{
			_mediator = mediator;
		}

		public void AddRoutes(IEndpointRouteBuilder app)
		{
			AddCreateProductEndpoint(app);
		}
	}
}