using Basket.Api.Features.ShoppingCarts.Queries.Retrievals;
using Services.Shared.CQRS;

namespace Basket.Api.Features.ShoppingCarts.Queries.Search;

public record SearchShoppingCart(string UserName, Guid? Id = null) : IQuery<ShoppingCartEntity?>;