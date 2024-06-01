using FluentValidation;
using MediatR;

namespace Services.Shared.Decorators
{
	public class ValidationDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly IValidator<TRequest> _validator;

		public ValidationDecorator(IValidator<TRequest> validator)
		{
			_validator = validator;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var validationResult = await _validator.ValidateAsync(request, cancellationToken);

			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			// Continue to the next handler in the pipeline
			return await next();
		}
	}
}