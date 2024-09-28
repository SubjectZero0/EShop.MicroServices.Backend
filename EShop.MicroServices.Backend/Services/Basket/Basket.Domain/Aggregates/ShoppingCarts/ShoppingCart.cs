using System.Text.Json.Serialization;

namespace Basket.Domain.Aggregates.ShoppingCarts;

public class ShoppingCart : AggregateRoot<Guid>
{
    [JsonInclude]
    public string UserName { get; }

    [JsonInclude]
    public List<ShoppingCartItem> Items { get; private set; }

    [JsonInclude]
    public DateTime CreatedAt { get; }
    
    [JsonInclude]
    public DateTime UpdatedAt { get; private set; }
    
    [JsonInclude]
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public ShoppingCart() : base(default) { }

    public ShoppingCart(Guid id, string userName, List<ShoppingCartItem> items, DateTime timeStamp) : base(id)
    {
        UserName = userName;
        Items = items;
        CreatedAt = timeStamp;
        UpdatedAt = timeStamp;
    }

    public static ShoppingCart CreateNew(Guid id, string userName, List<ShoppingCartItem> items, DateTime timeStamp)
    {
        return new ShoppingCart(
            id: id,
            userName: userName,
            items: items,
            timeStamp: timeStamp);
    }

    public void AddShoppingCartItem(ShoppingCartItem item, DateTime timeStamp)
    {
        Items.Add(item);
        UpdatedAt = timeStamp;
    }

    public void UpdateItems(ShoppingCartItem[] items)
    {
        foreach (var item in items)
        {
            var updatedItem = Items.FirstOrDefault(cartItem => cartItem.ProductId == item.ProductId);
            
            if (updatedItem is null)
                continue;
            
            updatedItem.Update(item.Quantity);
        }
    }
}