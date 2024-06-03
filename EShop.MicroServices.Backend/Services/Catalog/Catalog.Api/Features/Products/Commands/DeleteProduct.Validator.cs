using FluentValidation;

namespace Catalog.Api.Features.Products.Commands
{
	public class DeleteProductValidator : AbstractValidator<DeleteProduct>
	{
		public DeleteProductValidator()
		{
			RuleFor(x => x.Id).NotNull().NotEmpty();
		}
	}
}