using FluentValidation;

namespace Catalog.Api.Features.Products.Commands
{
	public class CreateProductValidator : AbstractValidator<CreateProduct>
	{
		public CreateProductValidator()
		{
			RuleFor(x => x.Name)
				.NotNull().WithMessage("Name cannot be null.")
				.NotEmpty().WithMessage("Name cannot be empty")
				.MaximumLength(255).WithMessage("Name cannot exceed 255 characters");

			RuleFor(x => x.Description)
				.MinimumLength(10).WithMessage("Description cannot be less than 10 characters.")
				.MaximumLength(255).WithMessage("Description cannot exceed 255 character.");

			RuleFor(x => x.Categories)
				.NotEmpty().WithMessage("Products must have Categories.");

			RuleFor(x => x.ImageFile)
				.NotEmpty().WithMessage("Products must have an Image file.");

			RuleFor(x => x.Price)
				.Must(price => IsMoney(price)).WithMessage("Price must have two decimal places.");
		}

		private static bool IsMoney(decimal price)
		{
			// Money has to have exactly two decimal places
			var decimals = price.ToString().Split(',').Last();

			return decimals.Length == 2;
		}
	}
}