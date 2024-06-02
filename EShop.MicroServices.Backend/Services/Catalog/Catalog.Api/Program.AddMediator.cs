using MediatR;
using Services.Shared.Decorators;
using System.Reflection;

namespace Catalog.Api
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddMediator(this WebApplicationBuilder builder)
		{
			builder.Services.AddMediatR(config =>
			{
				config.RegisterServicesFromAssembly(assembly: Assembly.GetExecutingAssembly());
				config.AddOpenBehavior(typeof(ValidationDecorator<,>));
				config.AddOpenBehavior(typeof(LoggingDecorator<,>));

				config.Lifetime = ServiceLifetime.Transient;
			});

			builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationDecorator<,>));

			return builder;
		}
	}
}