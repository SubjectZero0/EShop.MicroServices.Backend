using System.Text.Json;
using Basket.Domain.Aggregates.ShoppingCarts;
using Marten;
using Microsoft.Extensions.Caching.Distributed;
using Services.Shared.Storage;

namespace Basket.Api.Services;

public class ShoppingCartStorage : IStorage<ShoppingCart>
{
    private readonly IDistributedCache _cache;
    private readonly IDocumentSession _session;

    public ShoppingCartStorage(IDistributedCache cache, IDocumentSession session)
    {
        _cache = cache;
        _session = session;
    }

    public async Task Store(ShoppingCart entity)
    {
        var compositeKey = entity.UserName + "-" + $"{entity.Id}";
        
        _session.Store(entity);
        await _session.SaveChangesAsync();

        await _cache.SetStringAsync(compositeKey, JsonSerializer.Serialize(entity));
    }

    public async Task StoreUpdate(ShoppingCart entity)
    {
        var compositeKey = entity.UserName + "-" + $"{entity.Id}";
        
        _session.Update(entity);
        await _session.SaveChangesAsync();

        await _cache.SetStringAsync(compositeKey, JsonSerializer.Serialize(entity));
    }
}