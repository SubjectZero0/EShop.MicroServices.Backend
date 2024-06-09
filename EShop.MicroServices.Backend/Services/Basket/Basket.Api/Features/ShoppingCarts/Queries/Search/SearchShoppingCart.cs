using Basket.Domain.Aggregates.ShoppingCarts;
using Services.Shared.CQRS;

namespace Basket.Api.Features.ShoppingCarts.Queries.Search;

public record SearchShoppingCart(string UserName, Guid? Id = null) : IQuery<ShoppingCartEntity?>;

[Serializable]
public record ShoppingCartEntity(
    Guid Id,
    string UserName,
    List<ShoppingCartItem> Items,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    decimal TotalPrice);