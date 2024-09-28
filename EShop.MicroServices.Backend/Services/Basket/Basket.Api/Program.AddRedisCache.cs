using Basket.Api.Features.ShoppingCarts.Queries.Search;
using Basket.Api.Services;
using Basket.Domain.Aggregates.ShoppingCarts;
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
			.GetSection(nameof(Configurations.Configurations.RedisCacheConfiguration))
			.Get<Configurations.Configurations.RedisCacheConfiguration>() ?? throw new Exception("nameof(Configurations.Configurations.RedisCacheConfiguration) not found.");

		builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisCacheConfiguration.ConnectionString));

		builder.Services.AddStackExchangeRedisCache(options =>
		{
			options.Configuration = redisCacheConfiguration.ConnectionString;
		});

		builder.Services.AddSingleton(sp =>
		{
			var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
			return connectionMultiplexer.GetDatabase();
		});

		builder.Services.AddSingleton<IRetrieval<string, ShoppingCartEntity>, ShoppingCartRetrieval>();
		builder.Services.AddSingleton<IStorage<ShoppingCart>, ShoppingCartStorage>();
		//TODO: add lock mechanism
		return builder;
	}
}