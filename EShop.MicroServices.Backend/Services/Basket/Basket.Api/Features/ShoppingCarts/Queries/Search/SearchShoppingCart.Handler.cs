using Basket.Api.Constants;
using Basket.Api.Features.ShoppingCarts.Queries.Retrievals;
using Basket.Domain.Aggregates.ShoppingCarts;
using Marten;
using Services.Shared.CQRS;
using Services.Shared.Retrievals;

namespace Basket.Api.Features.ShoppingCarts.Queries.Search;

public class SearchShoppingCartHandler : IQueryHandler<SearchShoppingCart, ShoppingCartEntity?>
{
    private readonly IRetrieval<string, ShoppingCartEntity> _retrieval;
    private readonly IDocumentSession _session;

    public SearchShoppingCartHandler(IRetrieval<string, ShoppingCartEntity> retrieval, IDocumentSession session)
    {
        _retrieval = retrieval;
        _session = session;
    }

    public async Task<ShoppingCartEntity?> Handle(SearchShoppingCart request, CancellationToken cancellationToken)
    {
        var cachedEntities = _retrieval.RetrieveBatch([request.UserName]); //get from cache

        if (cachedEntities.Length > 0)
            return TryGetCartEntity(request, cachedEntities);

        var dbCarts = await GetCartsFromDb(request, cancellationToken);

        switch (dbCarts.Length)
        {
            case 0:
                return null;
            case > 0:
            {
                var cartEntities = dbCarts
                    .Select(CreateShoppingCartEntity)
                    .ToArray();
            
                return TryGetCartEntity(request, cartEntities);
            }
            default:
                return null;
        }
    }

    private static ShoppingCartEntity? TryGetCartEntity(SearchShoppingCart request, ShoppingCartEntity[] cartEntities)
    {
        return request.UserName switch
        {
            UserNames.DefaultUser when request.Id is null => null,
            UserNames.DefaultUser when request.Id is not null => cartEntities.FirstOrDefault(x => x.Id == request.Id),
            _ => cartEntities.FirstOrDefault()
        };
    }
    
    private static ShoppingCartEntity CreateShoppingCartEntity(ShoppingCart dbCart)
    {
        return new ShoppingCartEntity(
            Id: dbCart.Id, 
            UserName: dbCart.UserName,
            Items: dbCart.Items, 
            CreatedAt: dbCart.CreatedAt,
            UpdatedAt: dbCart.UpdatedAt, 
            TotalPrice: dbCart.TotalPrice);
    }
    
    private async Task<ShoppingCart[]> GetCartsFromDb(SearchShoppingCart request, CancellationToken cancellationToken)
    {
        var dbCarts = await _session
            .Query<ShoppingCart>()
            .Where(shoppingCart => shoppingCart.UserName == request.UserName)
            .ToListAsync(cancellationToken);

        return dbCarts.ToArray();
    }
}