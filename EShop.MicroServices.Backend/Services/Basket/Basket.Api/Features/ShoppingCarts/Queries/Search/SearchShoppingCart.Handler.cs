using Basket.Api.Constants;
using Services.Shared.CQRS;
using Services.Shared.Retrievals;

namespace Basket.Api.Features.ShoppingCarts.Queries.Search;

internal class SearchShoppingCartHandler : IQueryHandler<SearchShoppingCart, ShoppingCartEntity?>
{
	private readonly IRetrieval<string, ShoppingCartEntity> _retrieval;

	public SearchShoppingCartHandler(IRetrieval<string, ShoppingCartEntity> retrieval)
	{
		_retrieval = retrieval;
	}

	public async Task<ShoppingCartEntity?> Handle(SearchShoppingCart request, CancellationToken cancellationToken)
	{
		if (request is { UserName: UserNames.DefaultUser, CartId: null })
			return null;
		
		return await _retrieval.TryRetrieve(string.Concat(request.UserName, Separators.RedisKey, request.CartId), cancellationToken);
	}
}