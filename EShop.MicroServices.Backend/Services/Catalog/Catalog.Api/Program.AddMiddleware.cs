using Services.Shared.Middleware.Exceptions;

namespace Catalog.Api
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddMiddleware(this WebApplicationBuilder builder)
		{
			builder.Services.AddSingleton(typeof(ExceptionsMiddleware));
			return builder;
		}
	}
}