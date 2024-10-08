﻿using Microsoft.Extensions.Logging;
using RedLockNet;
using StackExchange.Redis;
using System.Text.Json;

namespace Services.Shared.Caching
{
	public class RedisCachingProvider<TKey, TEntity> : ICachingProvider<TKey, TEntity> where TKey : notnull where TEntity : class
	{
		private readonly IDatabase _cache;

		private readonly IDistributedLockFactory _redLockFactory;

		private readonly JsonSerializerOptions _jsonOptions;

		private readonly ILogger<RedisCachingProvider<TKey, TEntity>> _logger;

		public RedisCachingProvider(IDatabase cache, IDistributedLockFactory redLockFactory, ILogger<RedisCachingProvider<TKey, TEntity>> logger)
		{
			_cache = cache;
			_redLockFactory = redLockFactory;
			_logger = logger;

			_jsonOptions = new JsonSerializerOptions()
			{
				MaxDepth = 3,
				PropertyNameCaseInsensitive = true
			};
		}

		public async Task SetToCache(TKey key, TEntity entity)
		{
			try
			{
				var redisKey = new RedisKey(key.ToString());

				await using var redLock = await _redLockFactory.CreateLockAsync(
					resource: redisKey,
					expiryTime: TimeSpan.FromSeconds(10),
					waitTime: TimeSpan.FromSeconds(5),
					retryTime: TimeSpan.FromSeconds(1));

				if (redLock.IsAcquired)
				{
					var value = JsonSerializer.Serialize(entity);

					await _cache.StringSetAsync(
						key: redisKey,
						value: value,
						expiry: TimeSpan.FromHours(3));
				}
				else
				{
					_logger.LogWarning("Could not acquire a lock for Key: {key}.", redisKey);
				}
			}
			catch (Exception ex)
			{
				_logger
					.LogError("Something went wrong while trying to store entity to cache for Key: {key}, Message:{message}", key, ex.Message);

				await _cache.KeyDeleteAsync(key.ToString());
			}
		}

		public async Task<TEntity?> TryGetFromCache(TKey key)
		{
			try
			{
				var redisKey = new RedisKey(key.ToString());

				await using var redLock = await _redLockFactory.CreateLockAsync(
				resource: redisKey,
				expiryTime: TimeSpan.FromSeconds(10),
				waitTime: TimeSpan.FromSeconds(5),
				retryTime: TimeSpan.FromSeconds(1));

				if (redLock.IsAcquired)
				{
					var redisValue = await _cache.StringGetAsync(redisKey);

					return redisValue.HasValue
						? JsonSerializer.Deserialize<TEntity?>(redisValue.ToString(), _jsonOptions)
						: null;
				}
				else
				{
					_logger.LogWarning("Could not acquire a lock for Key: {key}.", redisKey);
					return null;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("Something went wrong while trying to retrieve entity from cache for Key: {key}, Message:{message}", key.ToString(), ex.Message);
				return null;
			}
		}
	}
}