using Services.Shared.Decorators;
using System.Reflection;

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
				config.AddOpenBehavior(typeof(ValidationDecorator<,>), ServiceLifetime.Singleton);
				config.AddOpenBehavior(typeof(LoggingDecorator<,>), ServiceLifetime.Singleton);

				config.Lifetime = ServiceLifetime.Singleton;
			});

			return builder;
		}
	}
}