using Basket.Domain.Aggregates.ShoppingCarts;
using Marten;
using RedLockNet;
using Services.Shared.Storage;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.Api.Services;

internal class ShoppingCartStorage : IStorage<ShoppingCart>
{
	private readonly IDatabase _cache;
	private readonly IDocumentSession _session;
	private readonly ILogger<ShoppingCartStorage> _logger;
	private readonly IDistributedLockFactory _redLockFactory;

	public ShoppingCartStorage(IDatabase cache, IDocumentStore store, ILogger<ShoppingCartStorage> logger, IDistributedLockFactory redLockFactory)
	{
		_cache = cache;
		_session = store.LightweightSession();
		_logger = logger;
		_redLockFactory = redLockFactory;
	}

	public async Task Store(ShoppingCart entity, CancellationToken ct)
	{
		if (!ct.IsCancellationRequested)
		{
			_session.Store(entity);
			await _session.SaveChangesAsync();
			await SetToCache(entity);
		}
	}

	public async Task StoreUpdate(ShoppingCart entity, CancellationToken ct)
	{
		if (!ct.IsCancellationRequested)
		{
			_session.Update(entity);
			await _session.SaveChangesAsync();
			await SetToCache(entity);
		}
	}

	private async Task SetToCache(ShoppingCart entity)
	{
		var compositeKey = string.Concat(entity.UserName, "-", entity.Id);

		await using var redLock = await _redLockFactory.CreateLockAsync(
				resource: compositeKey,
				expiryTime: TimeSpan.FromSeconds(30),
				waitTime: TimeSpan.FromSeconds(10),
				retryTime: TimeSpan.FromSeconds(1));

		if (redLock.IsAcquired)
		{
			try
			{
				var value = JsonSerializer.Serialize(entity);

				await _cache.StringSetAsync(key: compositeKey, value: value);
			}
			catch (Exception ex)
			{
				_logger.LogError($"ShoppingCart Entity with Id: {entity.Id} cound not be set to cache. {ex.Message}, {ex.StackTrace}");
				return;
			}
		}
	}
}