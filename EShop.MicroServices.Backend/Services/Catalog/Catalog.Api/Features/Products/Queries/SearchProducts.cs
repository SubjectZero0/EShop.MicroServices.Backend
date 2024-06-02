using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products.Queries
{
	public record SearchProducts(Guid? Id = null, string? Name = null, decimal? Price = null, string[]? Categories = null) : IQuery<SearchProductsEntity[]>;

	public record SearchProductsEntity(Guid Id, string Name, string Description, string ImageFile, decimal Price, IReadOnlyCollection<string> Categories);
}