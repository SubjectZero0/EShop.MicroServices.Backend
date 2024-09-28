using Catalog.Domain.Aggregates.Product;
using Marten;
using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products.Queries.Search
{
	internal class SearchProductsHandler : IQueryHandler<SearchProducts, SearchProductsEntity[]>
	{
		private readonly IQuerySession _dbSession;

		public SearchProductsHandler(IDocumentStore store)
		{
			_dbSession = store.QuerySession();
		}

		public async Task<SearchProductsEntity[]> Handle(SearchProducts request, CancellationToken cancellationToken)
		{
			var query = _dbSession.Query<Product>();

			if (request.Id is not null)
				query = (Marten.Linq.IMartenQueryable<Product>)query.Where(x => x.Id == request.Id.Value);

			if (request.Name is not null)
				query = (Marten.Linq.IMartenQueryable<Product>)query.Where(x => x.Name == request.Name);

			if (request.Price is not null)
				query = (Marten.Linq.IMartenQueryable<Product>)query.Where(x => x.Price == request.Price.Value);

			if (request.Categories is not null && request.Categories.Length > 0)
				query = (Marten.Linq.IMartenQueryable<Product>)query.Where(x => x.Categories.Any(_ => request.Categories.Contains(_)));

			var results = await query.ToListAsync(cancellationToken);

			var entities = results
				.Select(x => new SearchProductsEntity(
					Id: x.Id,
					Name: x.Name,
					Description: x.Description,
					ImageFile: x.ImageFile,
					Price: x.Price,
					Categories: x.Categories));

			return entities.ToArray();
		}
	}
}