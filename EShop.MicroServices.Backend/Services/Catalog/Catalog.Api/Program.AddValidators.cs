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
			builder.Services.AddSingleton<IValidator<CreateProduct>, CreateProductValidator>();
			builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationDecorator<,>));
			return builder;
		}
	}
}