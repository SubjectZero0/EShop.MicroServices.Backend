using FluentValidation;

namespace Catalog.Api.Features.Products
{
	public class CreateProductValidator : AbstractValidator<CreateProduct>
	{
		public CreateProductValidator()
		{
			RuleFor(x => x.Name)
				.NotNull()
				.NotEmpty();

			RuleFor(x => x.Description)
				.MinimumLength(10)
				.MaximumLength(150);

			RuleFor(x => x.Categories)
				.NotNull()
				.NotEmpty();

			RuleFor(x => x.ImageFile)
				.NotNull()
				.NotEmpty();

			RuleFor(x => x.Price)
				.NotNull()
				.Must(x => x.ToString().Split(".").Last().Length == 2);
		}
	}
}