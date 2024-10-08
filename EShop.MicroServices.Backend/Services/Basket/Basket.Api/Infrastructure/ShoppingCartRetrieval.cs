using Basket.Api.Constants;
using Basket.Api.Features.ShoppingCarts.Queries.Search;
using Basket.Domain.Aggregates.ShoppingCarts;
using Marten;
using Services.Shared.Caching;
using Services.Shared.Retrievals;
using System.Text.Json;

namespace Basket.Api.Infrastructure;

internal class ShoppingCartRetrieval : IRetrieval<string, ShoppingCartEntity>
{
	private readonly ICachingProvider<string, ShoppingCartEntity> _cachingProvider;
	private readonly IQuerySession _session;
	private readonly ILogger<ShoppingCartRetrieval> _logger;
	private readonly JsonSerializerOptions _jsonOptions;

	public ShoppingCartRetrieval(ICachingProvider<string, ShoppingCartEntity> cachingProvider, IDocumentStore store, ILogger<ShoppingCartRetrieval> logger)
	{
		_cachingProvider = cachingProvider;
		_session = store.QuerySession();
		_logger = logger;

		_jsonOptions = new JsonSerializerOptions()
		{
			MaxDepth = 3,
			PropertyNameCaseInsensitive = true
		};
	}

	public async Task<ShoppingCartEntity?> TryRetrieve(string searchParameter, CancellationToken ct)
	{
		if (ct.IsCancellationRequested)
			return null;

		var cachedEntity = await _cachingProvider.TryGetFromCache(searchParameter);

		if (cachedEntity is not null)
			return cachedEntity;

		return await GetFromDb(searchParameter);
	}

	public async Task<ShoppingCartEntity[]> RetrieveBatch(string[] searchParameters, CancellationToken ct)
	{
		var entities = new List<ShoppingCartEntity>();

		foreach (var searchParameter in searchParameters)
		{
			var retrievedEntity = await TryRetrieve(searchParameter, ct);

			if (retrievedEntity is not null)
				entities.Add(retrievedEntity);
		}

		return entities.ToArray();
	}

	private async Task<ShoppingCartEntity?> GetFromDb(string searchParameter)
	{
		var searchParameterSplit = searchParameter.Split(Separators.RedisKey);

		try
		{
			var userName = searchParameterSplit.First();
			var cartId = Guid.Parse(searchParameterSplit.Last());

			var dbCart = await _session
				.Query<ShoppingCart>()
				.Where(x => x.UserName == userName && x.Id == cartId)
				.ToJsonFirstOrDefault();

			return dbCart is null
				? null
				: JsonSerializer.Deserialize<ShoppingCartEntity>(dbCart, _jsonOptions);
		}
		catch (Exception ex)
		{
			_logger.LogError("Something went wrong while fetching cart from database. UserName: {username}, CartId: {id}, Message: {message}", searchParameterSplit.First(), searchParameterSplit.Last(), ex.Message);
			throw;
		}
	}
}