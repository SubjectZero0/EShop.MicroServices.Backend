using Basket.Domain.Aggregates.ShoppingCarts;
using Marten;
using Services.Shared.Storage;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.Api.Services;

internal class ShoppingCartStorage : IStorage<ShoppingCart>
{
	private readonly IDatabase _cache;
	private readonly IDocumentSession _session;

	public ShoppingCartStorage(IDatabase cache, IDocumentStore store)
	{
		_cache = cache;
		_session = store.LightweightSession();
	}

	public async Task Store(ShoppingCart entity)
	{
		_session.Store(entity);
		await _session.SaveChangesAsync();
		await SetToCache(entity);
	}

	public async Task StoreUpdate(ShoppingCart entity)
	{
		_session.Update(entity);
		await _session.SaveChangesAsync();
		await SetToCache(entity);
	}

	private async Task SetToCache(ShoppingCart entity)
	{
		var compositeKey = string.Concat(entity.UserName, "-", entity.Id);
		var value = JsonSerializer.Serialize(entity);

		await _cache.StringSetAsync(key: compositeKey, value: value);
	}
}