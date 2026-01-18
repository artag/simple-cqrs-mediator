using System.Net;
using System.Text.Json;

using FluentValidation;

namespace SimpleCqrsMediator.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var statusCode = HttpStatusCode.InternalServerError;
		var result = string.Empty;

		switch (exception)
		{
			case ValidationException validationException:
				statusCode = HttpStatusCode.BadRequest;
				var errors = validationException.Errors
					.GroupBy(e => e.PropertyName)
					.ToDictionary(
						g => g.Key,
						g => g.Select(e => e.ErrorMessage).ToArray()
					);
				result = JsonSerializer.Serialize(new
				{
					title = "Validation Failed",
					status = (int)statusCode,
					errors
				});
				break;
			case KeyNotFoundException:
				statusCode = HttpStatusCode.NotFound;
				result = JsonSerializer.Serialize(new
				{
					title = "Resource Not Found",
					status = (int)statusCode
				});
				break;
			default:
				logger.LogError(exception, "Unhandled exception occurred");
				result = JsonSerializer.Serialize(new
				{
					title = "An error occurred",
					status = (int)statusCode
				});
				break;
		}

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)statusCode;
		await context.Response.WriteAsync(result);
	}
}