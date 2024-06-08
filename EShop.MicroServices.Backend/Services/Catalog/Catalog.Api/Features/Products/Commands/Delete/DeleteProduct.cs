using MediatR;
using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products.Commands.Delete
{
	public record DeleteProduct(Guid Id) : ICommand<Unit>;
}