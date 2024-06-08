using FluentValidation;

namespace Catalog.Api.Features.Products.Queries.Search
{
	public class SearchProductsValidator : AbstractValidator<SearchProducts>
	{
		public SearchProductsValidator()
		{ }
	}
}