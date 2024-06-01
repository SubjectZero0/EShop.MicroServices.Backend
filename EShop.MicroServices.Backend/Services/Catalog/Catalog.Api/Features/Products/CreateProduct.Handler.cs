using Catalog.Domain.Aggregates.Product;
using MediatR;
using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products
{
	internal class CreateProductHandler : ICommandHandler<CreateProduct, Unit>
	{
		public async Task<Unit> Handle(CreateProduct request, CancellationToken cancellationToken)
		{
			var newProduct = Product.CreateNew(
				name: request.Name,
				description: request.Description,
				imageFile: request.ImageFile,
				price: request.Price,
				categories: request.Categories);

			//TODO: save to db

			return Unit.Value;
		}
	}
}