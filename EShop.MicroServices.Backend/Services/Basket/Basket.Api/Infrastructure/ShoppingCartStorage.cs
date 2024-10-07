using Basket.Domain.Aggregates.ShoppingCarts;
using Services.Shared.Caching;
using Services.Shared.Storage;
using Marten;
using Basket.Api.Constants;

namespace Basket.Api.Infrastructure;

internal class ShoppingCartStorage : IStorage<ShoppingCart>
{
	private readonly IDocumentSession _session;
	private readonly ILogger<ShoppingCartStorage> _logger;
	private readonly ICachingProvider<string, ShoppingCart> _cachingProvider;

	public ShoppingCartStorage(IDocumentStore store, ILogger<ShoppingCartStorage> logger, ICachingProvider<string, ShoppingCart> cachingProvider)
	{
		_session = store.LightweightSession();
		_logger = logger;
		_cachingProvider = cachingProvider;
	}

	public async Task StoreNew(ShoppingCart entity, CancellationToken ct)
	{
		if (ct.IsCancellationRequested)
			return;

		var compositeKey = string.Concat(entity.UserName, Separators.RedisKey, entity.Id);

		try
		{
			_session.Store(entity);
			await _session.SaveChangesAsync(ct);
		}
		catch (Exception ex)
		{
			_logger.LogError("ShoppingCart Entity with Id: {Id} cound not be stored. {Message}, {StackTrace}", entity.Id, ex.Message, ex.StackTrace);
			return;
		}

		await _cachingProvider.SetToCache(compositeKey, entity);
	}

	public async Task StoreUpdate(ShoppingCart entity, CancellationToken ct)
	{
		if (ct.IsCancellationRequested)
			return;

		var compositeKey = string.Concat(entity.UserName, Separators.RedisKey, entity.Id);

		try
		{
			_session.Update(entity);
			await _session.SaveChangesAsync(ct);
		}
		catch (Exception ex)
		{
			_logger.LogError("ShoppingCart Entity with Id: {Id} cound not be updated. {Message}, {StackTrace}", entity.Id, ex.Message, ex.StackTrace);
			return;
		}

		await _cachingProvider.SetToCache(compositeKey, entity);
	}
}