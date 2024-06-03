using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Services.Shared.Decorators
{
	public class LoggingDecorator<TRequest, TResponse>(ILogger<LoggingDecorator<TRequest, TResponse>> logger)
		: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var timer = new Stopwatch();

			timer.Start();

			var response = await next();

			timer.Stop();

			if (timer.Elapsed.Seconds > 2)
				logger.LogWarning("[PERFORMANCE ISSUE] The request {Request} took {TimeTaken} seconds.",
					typeof(TRequest).Name, timer.Elapsed.Seconds);

			return response;
		}
	}
}