using MediatR;
using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products.Commands
{
	public record UpdateProduct(
		Guid Id,
		string? Name = null,
		string? Description = null,
		string? ImageFile = null,
		decimal? Price = null,
		string[]? AddCategories = null,
		string[]? RemoveCategories = null) : ICommand<Unit>;
}