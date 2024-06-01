using Catalog.Api.Features.Products;
using FluentValidation;
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
			});

			return builder;
		}
	}
}