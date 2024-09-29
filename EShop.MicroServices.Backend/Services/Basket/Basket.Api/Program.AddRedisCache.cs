using Basket.Api.Features.ShoppingCarts.Queries.Search;
using Basket.Api.Services;
using Basket.Domain.Aggregates.ShoppingCarts;
using RedLockNet.SERedis;
using RedLockNet;
using Services.Shared.Retrievals;
using Services.Shared.Storage;
using StackExchange.Redis;
using RedLockNet.SERedis.Configuration;
using Services.Shared.Extensions;

namespace Basket.Api;

public static partial class Program
{
	public static WebApplicationBuilder AddRedisCache(this WebApplicationBuilder builder)
	{
		var redisCacheConfiguration = builder
			.Configuration
			.GetSection(nameof(Configurations.Configurations.RedisCacheConfiguration))
			.Get<Configurations.Configurations.RedisCacheConfiguration>() ?? throw new Exception("nameof(Configurations.Configurations.RedisCacheConfiguration) not found.");

		var configurationOptions = ConfigurationOptions.Parse(redisCacheConfiguration.ConnectionString);
		configurationOptions.AllowAdmin = true;
		configurationOptions.ConnectRetry = 5;
		configurationOptions.ConnectTimeout = 5000;
		configurationOptions.SyncTimeout = 10000;
		configurationOptions.AsyncTimeout = 10000;
		configurationOptions.AbortOnConnectFail = false;
		configurationOptions.KeepAlive = 10;
		configurationOptions.ReconnectRetryPolicy = new ExponentialRetry(5000);
		configurationOptions.AbortOnConnectFail = false;

		builder.Services.AddStackExchangeRedisCache(options =>
		{
			options.ConfigurationOptions = configurationOptions;
		});

		builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisCacheConfiguration.ConnectionString));

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

		builder.Services.AddSingleton<IRetrieval<string, ShoppingCartEntity>, ShoppingCartRetrieval>();
		builder.Services.AddSingleton<IStorage<ShoppingCart>, ShoppingCartStorage>();

		return builder;
	}
}