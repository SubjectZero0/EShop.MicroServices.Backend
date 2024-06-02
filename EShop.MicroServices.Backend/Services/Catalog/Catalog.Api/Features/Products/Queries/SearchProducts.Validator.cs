using FluentValidation;

namespace Catalog.Api.Features.Products.Queries
{
	public class SearchProductsValidator : AbstractValidator<SearchProducts>
	{
		public SearchProductsValidator()
		{ }
	}
}