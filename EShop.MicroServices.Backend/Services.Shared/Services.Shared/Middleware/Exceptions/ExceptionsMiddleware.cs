using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Services.Shared.Middleware.Exceptions
{
	public class ExceptionsMiddleware : IMiddleware
	{
		private readonly ILogger<ExceptionsMiddleware> _logger;

		public ExceptionsMiddleware(ILogger<ExceptionsMiddleware> logger)
		{
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception exception)
			{
				_logger.LogError("Timestamp:{timestamp}, ErrorMessage:{exceptionMessage}, StackTrace: {stacktrace}", DateTime.Now, exception.Message, exception.StackTrace);

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				var errorResponse = new
				{
					ErrorMessage = "An error occurred. Please try again later.",
					ErrorCode = 500
				};

				await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
			}
		}
	}
}