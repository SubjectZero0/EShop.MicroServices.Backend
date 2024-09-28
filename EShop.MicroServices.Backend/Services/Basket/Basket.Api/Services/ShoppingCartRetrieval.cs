using Basket.Api.Features.ShoppingCarts.Queries.Search;
using Basket.Domain.Aggregates.ShoppingCarts;
using Marten;
using Services.Shared.Retrievals;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.Api.Services;

internal class ShoppingCartRetrieval : IRetrieval<string, ShoppingCartEntity>
{
	private readonly IDatabase _cache;
	private readonly IQuerySession _session;
	private readonly ILogger<ShoppingCartRetrieval> _logger;
	private readonly JsonSerializerOptions _jsonOptions;

	public ShoppingCartRetrieval(IDocumentStore store, ILogger<ShoppingCartRetrieval> logger, IDatabase cache)
	{
		_session = store.QuerySession();
		_logger = logger;
		_cache = cache;

		_jsonOptions = new JsonSerializerOptions()
		{
			MaxDepth = 3,
			PropertyNameCaseInsensitive = true
		};
	}

	public async Task<ShoppingCartEntity?> TryRetrieve(string searchParameter)
	{
		try
		{
			var redisValue = await _cache.StringGetAsync(searchParameter);
			var cachedCartString = redisValue.HasValue ? redisValue.ToString() : null;

			if (cachedCartString is null)
				return null;

			var cachedEntity = JsonSerializer.Deserialize<ShoppingCartEntity>(cachedCartString, _jsonOptions);

			return cachedEntity;
		}
		catch (Exception ex)
		{
			_logger.LogError("Something went wrong while trying to retrieve Shopping carts from cache for UserName: {username}, Message:{message}", searchParameter, ex.Message);
		}

		var userName = searchParameter.Split("-").First();
		var cartId = Guid.Parse(searchParameter.Split("-").Last());

		var dbCart = _session
			.Query<ShoppingCart>()
			.SingleOrDefault(x => x.UserName == userName && x.Id == cartId);

		return dbCart is null
			? null
			: new ShoppingCartEntity(
				Id: dbCart.Id,
				UserName: dbCart.UserName,
				Items: dbCart.Items,
				CreatedAt: dbCart.CreatedAt,
				UpdatedAt: dbCart.UpdatedAt,
				TotalPrice: dbCart.TotalPrice);
	}

	public async Task<ShoppingCartEntity[]> RetrieveBatch(string[] searchParameters)
	{
		List<ShoppingCartEntity> entities = new();

		foreach (var searchParameter in searchParameters)
		{
			var retrievedEntity = await TryRetrieve(searchParameter);

			if (retrievedEntity is not null)
				entities.Add(retrievedEntity);
		}

		return entities.ToArray();
	}
}