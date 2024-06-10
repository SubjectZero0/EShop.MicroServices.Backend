using Basket.Api.Constants;
using Basket.Api.Services;
using Services.Shared.CQRS;
using Services.Shared.Retrievals;

namespace Basket.Api.Features.ShoppingCarts.Queries.Search;

internal class SearchShoppingCartHandler : IQueryHandler<SearchShoppingCart, ShoppingCartEntity?>
{
    private readonly IRetrieval<string, ShoppingCartEntity[]> _retrieval;

    public SearchShoppingCartHandler(IRetrieval<string, ShoppingCartEntity[]> retrieval)
    {
        _retrieval = retrieval;
    }

    public async Task<ShoppingCartEntity?> Handle(SearchShoppingCart request, CancellationToken cancellationToken)
    {
        var cartEntities = await _retrieval.TryRetrieve(request.UserName);

        return cartEntities.Length > 0 ? TryGetCartEntity(request, cartEntities) : null;
    }

    private static ShoppingCartEntity? TryGetCartEntity(SearchShoppingCart request, ShoppingCartEntity[] cartEntities)
    {
        return request.UserName == UserNames.DefaultUser
            ? cartEntities.FirstOrDefault(x => x.Id == request.Id)
            : cartEntities.FirstOrDefault();
    }
}