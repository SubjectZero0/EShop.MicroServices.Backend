using System.Text.Json;
using Basket.Domain.Aggregates.ShoppingCarts;
using Marten;
using Microsoft.Extensions.Caching.Distributed;
using Services.Shared.Storage;

namespace Basket.Api.Services;

internal class ShoppingCartStorage : IStorage<ShoppingCart>
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
        var value = JsonSerializer.Serialize(entity);
        
        _session.Store(entity);
        await _session.SaveChangesAsync();

        await _cache.SetStringAsync(compositeKey, value);
    }

    public async Task StoreUpdate(ShoppingCart entity)
    {
        var compositeKey = entity.UserName + "-" + $"{entity.Id}";
        var value = JsonSerializer.Serialize(entity);
        
        _session.Update(entity);
        await _session.SaveChangesAsync();

        await _cache.SetStringAsync(compositeKey, value);
    }
}