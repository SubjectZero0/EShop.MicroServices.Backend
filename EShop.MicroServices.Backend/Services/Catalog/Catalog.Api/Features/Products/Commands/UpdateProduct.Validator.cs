using FluentValidation;

namespace Catalog.Api.Features.Products.Commands
{
	public class UpdateProductValidator : AbstractValidator<UpdateProduct>
	{
		public UpdateProductValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().When(x => x.Name is not null).WithMessage("If Name is included, it cannot be empty")
				.MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

			RuleFor(x => x.Description)
				.MinimumLength(10).When(x => x.Description is not null).WithMessage("Description cannot be less than 10 characters.")
				.MaximumLength(255).When(x => x.Description is not null).WithMessage("Description cannot exceed 255 characters.");

			RuleFor(x => x.AddCategories)
				.NotEmpty().When(x => x.AddCategories is not null).WithMessage("If Added Categories are included, they cannot be empty.");

			RuleFor(x => x.RemoveCategories)
				.NotEmpty().When(x => x.RemoveCategories is not null).WithMessage("If Removed Categories are included, they cannot be empty.");

			RuleFor(x => x.ImageFile)
				.NotEmpty().When(x => x.ImageFile is not null).WithMessage("If Image file is included, it cannot be empty.");

			RuleFor(x => x.Price)
				.Must(price => IsMoney(price)).When(x => x.Price is not null).WithMessage("If Price is included, it must have two decimal places.");
		}

		private static bool IsMoney(decimal? price)
		{
			// Money has to have exactly two decimal places
			var decimals = price?.ToString().Split(',').Last();

			return decimals?.Length == 2;
		}
	}
}