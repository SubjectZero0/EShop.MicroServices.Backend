using FluentValidation;
using System.Reflection;

namespace Basket.Api
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