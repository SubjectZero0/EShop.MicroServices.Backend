﻿using System.Reflection;
using Services.Shared.Decorators;

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

			return builder;
		}
	}
}