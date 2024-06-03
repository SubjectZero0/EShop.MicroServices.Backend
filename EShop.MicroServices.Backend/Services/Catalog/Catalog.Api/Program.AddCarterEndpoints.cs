using Carter;
using Catalog.Api.Features.Products;

namespace Catalog.Api
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddCarterEndpoints(this WebApplicationBuilder builder)
		{
			builder.Services.AddCarter(configurator: cfg =>
			{
				cfg
				.WithModule<ProductModule>();
			});

			return builder;
		}
	}
}