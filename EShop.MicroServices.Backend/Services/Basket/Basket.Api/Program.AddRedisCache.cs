using Basket.Api.Configurations;
using Basket.Api.Features.ShoppingCarts.Queries.Search;
using Basket.Api.Infrastructure;
using Basket.Domain.Aggregates.ShoppingCarts;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using Services.Shared.Caching;
using Services.Shared.Retrievals;
using Services.Shared.Storage;
using StackExchange.Redis;

namespace Basket.Api;

public static partial class Program
{
	public static WebApplicationBuilder AddRedisCache(this WebApplicationBuilder builder)
	{
		var redisCacheConfiguration = builder
			.Configuration
			.GetSection(nameof(RedisCacheConfiguration))
			.Get<RedisCacheConfiguration>() ?? throw new Exception("nameof(Configurations.Configurations.RedisCacheConfiguration) not found.");

		var redisConnectionString = redisCacheConfiguration.ConnectionString;

		var configurationOptions = ConfigurationOptions.Parse(redisConnectionString);
		configurationOptions.AllowAdmin = redisCacheConfiguration.AllowAdmin;
		configurationOptions.DefaultDatabase = redisCacheConfiguration.DefaultDb;

		builder.Services.AddStackExchangeRedisCache(options =>
		{
			options.ConfigurationOptions = configurationOptions;
		});

		builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnectionString));

		builder.Services.AddSingleton(sp =>
		{
			var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
			return connectionMultiplexer.GetDatabase();
		});

		builder.Services.AddSingleton<IDistributedLockFactory>(sp =>
		{
			var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
			return RedLockFactory.Create(connectionMultiplexer.GetEndPoints().Select(ep => new RedLockEndPoint(ep)).ToList());
		});

		builder.Services.AddSingleton<ICachingProvider<string, ShoppingCart>, RedisCachingProvider<string, ShoppingCart>>();
		builder.Services.AddSingleton<ICachingProvider<string, ShoppingCartEntity>, RedisCachingProvider<string, ShoppingCartEntity>>();

		builder.Services.AddSingleton<IRetrieval<string, ShoppingCartEntity>, ShoppingCartRetrieval>();
		builder.Services.AddSingleton<IStorage<ShoppingCart>, ShoppingCartStorage>();

		return builder;
	}
}