using Carter;

namespace Basket.Api
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddCarterEndpoints(this WebApplicationBuilder builder)
		{
			builder.Services.AddCarter(configurator: cfg =>
			{
				//cfg.WithModule<>();
			});

			return builder;
		}
	}
}