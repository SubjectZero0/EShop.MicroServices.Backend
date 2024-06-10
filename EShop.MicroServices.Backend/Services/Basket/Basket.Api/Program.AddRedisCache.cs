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
           .Get<Configurations.Configurations.RedisCacheConfiguration>() ?? throw new ArgumentNullException(nameof(Configurations.Configurations.RedisCacheConfiguration));
        
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            // options.Configuration = redisCacheConfiguration.ConnectionString;
            //options.InstanceName = "Basket";
            options.ConfigurationOptions = new ConfigurationOptions()
            {
                EndPoints = {redisCacheConfiguration.Host, redisCacheConfiguration.Port},
                AllowAdmin = true
            };
        });
        
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse(redisCacheConfiguration.ConnectionString, true);
            configuration.AllowAdmin = true;
            
            var redis =  ConnectionMultiplexer.Connect(configuration);
           
            return redis;
        });
        
        builder.Services.AddTransient<IDatabase>(sp =>
        {
            var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
            return connectionMultiplexer.GetDatabase();
        });
        
        builder.Services.AddTransient<RedisKeyScanner>();
        builder.Services.AddTransient<IRetrieval<string, ShoppingCartEntity[]>, ShoppingCartRetrieval>();
        builder.Services.AddTransient<IStorage<ShoppingCart>, ShoppingCartStorage>();
        //TODO: add lock mechanism
        return builder;
    }
}