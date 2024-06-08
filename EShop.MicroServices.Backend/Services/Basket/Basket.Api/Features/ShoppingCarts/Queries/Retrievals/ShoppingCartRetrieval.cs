using Services.Shared.Retrievals;

namespace Basket.Api.Features.ShoppingCarts.Queries.Retrievals;

internal class ShoppingCartRetrieval : IRetrieval<string, ShoppingCartEntity>
{
    public ShoppingCartEntity? TryRetrieve(string searchParameter)
    {
        throw new NotImplementedException();
    }

    public ShoppingCartEntity[] RetrieveBatch(string[] searchParameters)
    {
        throw new NotImplementedException();
    }
}