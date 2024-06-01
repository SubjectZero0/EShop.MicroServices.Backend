using Catalog.Api.Features.Products;
using FluentValidation;
using MediatR;
using Services.Shared.Decorators;

namespace Catalog.Api
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddValidators(this WebApplicationBuilder builder)
		{
			builder.Services.AddValidatorsFromAssembly(assembly: Assembly.GetExecutingAssembly(), lifetime: ServiceLifetime.Transient);

			return builder;
		}
	}
}