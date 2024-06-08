using FluentValidation;

namespace Catalog.Api.Features.Products.Commands.Delete
{
	public class DeleteProductValidator : AbstractValidator<DeleteProduct>
	{
		public DeleteProductValidator()
		{
			RuleFor(x => x.Id).NotNull().NotEmpty();
		}
	}
}