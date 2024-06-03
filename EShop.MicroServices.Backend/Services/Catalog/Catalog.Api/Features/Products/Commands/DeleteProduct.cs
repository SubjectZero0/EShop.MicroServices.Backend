using MediatR;
using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products.Commands
{
	public record DeleteProduct(Guid Id) : ICommand<Unit>;
}