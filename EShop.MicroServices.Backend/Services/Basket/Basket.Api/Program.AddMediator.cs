using System.Reflection;
using Basket.Api.Features.ShoppingCarts.Queries.Search;
using Basket.Api.Services;
using Basket.Domain.Aggregates.ShoppingCarts;
using Services.Shared.Decorators;
using Services.Shared.Retrievals;
using Services.Shared.Storage;

namespace Basket.Api
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddMediator(this WebApplicationBuilder builder)
		{
            builder.Services.AddMediatR(config =>
            {
	            var assembly = Assembly.GetExecutingAssembly();
				config.RegisterServicesFromAssembly(assembly: assembly);
				config.AddOpenBehavior(typeof(ValidationDecorator<,>), ServiceLifetime.Transient);
				config.AddOpenBehavior(typeof(LoggingDecorator<,>), ServiceLifetime.Transient);

				config.Lifetime = ServiceLifetime.Transient;
			});
			
			return builder;
		}
	}
}