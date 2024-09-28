using System.Text.Json;
using Catalog.Domain.Aggregates.Product;
using Marten;
using MediatR;
using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products.Commands.Update
{
	internal class UpdateProductHandler : ICommandHandler<UpdateProduct, Unit>
	{
		private readonly IDocumentSession _dbSession;
		private readonly ILogger<UpdateProductHandler> _logger;

		public UpdateProductHandler(IDocumentStore store, ILogger<UpdateProductHandler> logger)
		{
			_dbSession = store.LightweightSession();
			_logger = logger;
		}

		public async Task<Unit> Handle(UpdateProduct request, CancellationToken cancellationToken)
		{
			var product = await _dbSession.LoadAsync<Product>(request.Id, cancellationToken);

			if (product is null)
			{
				_logger.LogWarning("Product with Id: {id} could not be updated. Product doesnt exist.", request.Id);
				return Unit.Value;
			}

			product.Update(
				name: request.Name,
				description: request.Description,
				imageFile: request.ImageFile,
				price: request.Price);

			if (request.AddCategories is not null && request.AddCategories.Length >= 1)
				product.AddCategories(request.AddCategories);

			if (request.RemoveCategories is not null && request.RemoveCategories.Length >= 1)
				product.RemoveCategories(request.RemoveCategories);

			_dbSession.Update(product);
			await _dbSession.SaveChangesAsync(cancellationToken);

			_logger.LogInformation("Product with Id: {id} updated. {product}", product.Id, JsonSerializer.Serialize(product));

			return Unit.Value;
		}
	}
}