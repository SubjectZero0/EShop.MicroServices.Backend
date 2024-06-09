using System.Text.Json;
using Basket.Api.Constants;
using Basket.Api.Features.ShoppingCarts.Queries.Search;
using Basket.Domain.Aggregates.ShoppingCarts;
using Marten;
using Microsoft.Extensions.Caching.Distributed;
using Services.Shared.Retrievals;
using StackExchange.Redis;

namespace Basket.Api.Services;

public class ShoppingCartRetrieval : IRetrieval<string, ShoppingCartEntity[]>
{
    private readonly IDistributedCache _cache;
    private readonly IQuerySession _session;
    private readonly RedisKeyScanner _scanner;
    private readonly ILogger<ShoppingCartRetrieval> _logger;

    private static JsonSerializerOptions DeserializerOptions()
    {
        return new JsonSerializerOptions()
        {
            MaxDepth = 3,
            PropertyNameCaseInsensitive = true
        };
    }
    public ShoppingCartRetrieval(IDistributedCache cache, IQuerySession session, RedisKeyScanner scanner, ILogger<ShoppingCartRetrieval> logger)
    {
        _cache = cache;
        _session = session;
        _scanner = scanner;
        _logger = logger;
    }
    
    public async Task<ShoppingCartEntity[]> TryRetrieve(string searchParameter)
    {
        if (searchParameter != UserNames.DefaultUser)
            return await RetrieveUserCart(searchParameter);

        return await RetrieveDefaultUserCarts();
    }

    public async Task<ShoppingCartEntity[]> RetrieveBatch(string[] searchParameters)
    {
        List<ShoppingCartEntity> entities = new();
        foreach (var searchParameter in searchParameters)
        {
            var retrievedEntities = await TryRetrieve(searchParameter);
            entities.AddRange(retrievedEntities);
        }

        return entities.ToArray();
    }
    
    private async Task<ShoppingCartEntity[]> RetrieveDefaultUserCarts()
    {
        try
        {
            var cachedCartStrings = await _scanner.GetRedisStrings(UserNames.DefaultUser);
            return GetCachedEntities(cachedCartStrings);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while trying to retrieve Shopping carts from cache for DefaultUser, Message:{message}", ex.Message);
        }

        var dbCarts = await _session
            .Query<ShoppingCart>()
            .Where(x => x.UserName == UserNames.DefaultUser)
            .ToListAsync();

        
        return dbCarts.IsEmpty()
            ? Array.Empty<ShoppingCartEntity>()
            : dbCarts
                .Select(dbCart => new ShoppingCartEntity(
                    Id: dbCart.Id,
                    UserName: dbCart.UserName,
                    Items: dbCart.Items,
                    CreatedAt: dbCart.CreatedAt,
                    UpdatedAt: dbCart.UpdatedAt,
                    TotalPrice: dbCart.TotalPrice))
                .ToArray();
    }

    private static ShoppingCartEntity[] GetCachedEntities(string[] cachedCartStrings)
    {
        var cachedCartEntities = new List<ShoppingCartEntity>();

        foreach (var cachedCartString in cachedCartStrings)
        {
            if (string.IsNullOrEmpty(cachedCartString) || string.IsNullOrWhiteSpace(cachedCartString)) 
                continue;
                
            var cachedEntity = GetCachedEntity(cachedCartString);
                    
            if (cachedEntity is not null)
                cachedCartEntities.Add(cachedEntity);
        }

        return cachedCartEntities.ToArray();
    }

    private async Task<ShoppingCartEntity[]> RetrieveUserCart(string searchParameter)
    {
        try
        {
            var cachedCartString = await _cache.GetStringAsync(searchParameter) ?? string.Empty;

            if (!string.IsNullOrEmpty(cachedCartString))
            {
                var cachedEntity = GetCachedEntity(cachedCartString);

                if (cachedEntity is not null)
                    return [cachedEntity];
            }
        }
        catch(Exception ex)
        {
            _logger.LogError("Something went wrong while trying to retrieve Shopping carts from cache for UserName: {username}, Message:{message}", searchParameter, ex.Message);
        }
        
        var dbCart = _session
            .Query<ShoppingCart>()
            .SingleOrDefault(x => x.UserName == searchParameter);
        
        return dbCart is null
            ? Array.Empty<ShoppingCartEntity>()
            :
            [
                new ShoppingCartEntity(
                    Id: dbCart.Id,
                    UserName: dbCart.UserName,
                    Items: dbCart.Items,
                    CreatedAt: dbCart.CreatedAt,
                    UpdatedAt: dbCart.UpdatedAt,
                    TotalPrice: dbCart.TotalPrice)
            ];
    }
    
    private static ShoppingCartEntity? GetCachedEntity(string cachedCartString)
        => JsonSerializer.Deserialize<ShoppingCartEntity>(cachedCartString, DeserializerOptions());
}