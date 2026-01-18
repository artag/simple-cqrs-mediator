using Microsoft.Extensions.Logging;

namespace SimpleCqrsMediator.Core.CQRS.Behaviors;

internal class LoggingBehavior<TInput, TOutput>(ILogger<LoggingBehavior<TInput, TOutput>> logger)
	: IPipelineBehavior<TInput, TOutput>
{
	public async Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken)
	{
		logger.LogInformation("Starting: {TypeName}", typeof(TInput).Name);

		var result = await next();

		logger.LogInformation("Completed: {TypeName}", typeof(TOutput).Name);

		return result;
	}
}