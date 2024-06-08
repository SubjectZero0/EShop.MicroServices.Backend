using System.Text.Json.Serialization;

namespace Basket.Domain.Aggregates.ShoppingCarts;

public class ShoppingCartItem : ValueObject
{
    [JsonInclude]
    public int Quantity { get; private set; } 
    
    [JsonInclude]
    public string Color { get; }
    
    [JsonInclude]
    public decimal Price { get; }
    
    [JsonInclude]
    public Guid ProductId { get; }
    
    [JsonInclude]
    public string ProductName { get; }

    public ShoppingCartItem() { }

    public ShoppingCartItem(int quantity, string color, decimal price, Guid productId, string productName)
    {
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductId = productId;
        ProductName = productName;
    }

    public static ShoppingCartItem CreateNew(int quantity, string color, decimal price, Guid productId, string productName)
    {
        return new ShoppingCartItem(
            quantity: quantity,
            color: color,
            price: price,
            productId: productId,
            productName: productName);
    }

    public void Update(int quantity)
    {
        Quantity = quantity;
    }
}