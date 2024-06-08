using Catalog.Domain.Aggregates.Product;
using Marten;
using MediatR;
using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products.Commands.Create
{
	internal class CreateProductHandler : ICommandHandler<CreateProduct, Unit>
	{
		private readonly IDocumentSession _dbSession;

		public CreateProductHandler(IDocumentSession dbSession)
		{
			_dbSession = dbSession;
		}

		public async Task<Unit> Handle(CreateProduct request, CancellationToken cancellationToken)
		{
			var newProduct = Product.CreateNew(
				name: request.Name,
				description: request.Description,
				imageFile: request.ImageFile,
				price: request.Price,
				categories: request.Categories);

			_dbSession.Store(newProduct);
			await _dbSession.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}