using MediatR;
using Services.Shared.CQRS;

namespace Catalog.Api.Features.Products.Commands.Create
{
	public record CreateProduct(string Name, string Description, string ImageFile, decimal Price, string[] Categories) : ICommand<Unit>;
}