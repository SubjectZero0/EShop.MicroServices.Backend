using Catalog.Domain.Aggregates.Product;
using Marten;
using MediatR;
using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products.Commands
{
	internal class DeleteProductHandler : ICommandHandler<DeleteProduct, Unit>
	{
		private readonly IDocumentSession _dbSession;
		private readonly ILogger<DeleteProductHandler> _logger;

		public DeleteProductHandler(IDocumentSession dbSession, ILogger<DeleteProductHandler> logger)
		{
			_dbSession = dbSession;
			_logger = logger;
		}

		public async Task<Unit> Handle(DeleteProduct request, CancellationToken cancellationToken)
		{
			var product = await _dbSession.LoadAsync<Product>(request.Id, cancellationToken);

			if (product is null)
			{
				_logger.LogWarning("Product with Id: {id} could not be deleted. Product not found.", request.Id);
				return Unit.Value;
			}

			_dbSession.Delete(product);
			await _dbSession.SaveChangesAsync();

			return Unit.Value;
		}
	}
}