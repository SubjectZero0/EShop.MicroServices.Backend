using Marten;
using StackExchange.Redis;

namespace Basket.Api.Services;

internal class RedisKeyScanner
{
    private readonly IDatabase _redisDatabase;

    public RedisKeyScanner(IDatabase redisDatabase)
    {
        _redisDatabase = redisDatabase;
    }

    public async Task<string[]> GetRedisStrings(string pattern)
    {
        var redisValues = new List<string>();

        // Use SCAN command to iterate over keys matching the substring pattern
        var redisKeys = await ScanKeys(pattern);

        // Retrieve values for matched keys
        foreach (var key in redisKeys)
        {
            var hashSets = await _redisDatabase.HashGetAllAsync(key);
            var redisValue = hashSets.First().Value;
            
            redisValues.Add(redisValue.ToString());
        }

        return redisValues.ToArray();
    }

    private async Task<RedisKey[]> ScanKeys(string pattern)
    {
        var keys = new List<RedisKey>();
        long cursor = 0;
        do
        { 
            var result = await _redisDatabase.ExecuteAsync("SCAN", cursor.ToString()!, "MATCH", "*" + pattern + "*");

            if (result.IsNull)
                return keys.ToArray();
            
            var innerResult = (RedisResult[]?)result;
            cursor = innerResult is null || innerResult.Length == 0  ? 0 : (long)innerResult[0];
            
            foreach (var key in (RedisResult[])innerResult[1])
                keys.Add(key.ToString());
            
        } while (cursor != 0);

        return keys.ToArray();
    }
}