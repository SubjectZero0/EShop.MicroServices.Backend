using Basket.Domain.Aggregates.ShoppingCarts;

namespace Basket.Api.Features.ShoppingCarts.Queries.Retrievals;

public record ShoppingCartEntity(
    Guid Id,
    string UserName,
    List<ShoppingCartItem> Items,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    decimal TotalPrice);