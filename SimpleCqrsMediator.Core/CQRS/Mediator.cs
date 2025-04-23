using Microsoft.Extensions.DependencyInjection;

namespace SimpleCqrsMediator.Core.CQRS;

internal class Mediator(IServiceProvider provider) : IMediator
{
	private readonly IServiceProvider _provider = provider;

	public async Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
		where TCommand : ICommand<TResult>
	{
		var handler = _provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();

		var behaviors = _provider.GetServices<IPipelineBehavior<TCommand, TResult>>().Reverse();
		Func<Task<TResult>> handlerDelegate = () => handler.HandleAsync(command, cancellationToken);
		foreach (var behavior in behaviors)
		{
			var next = handlerDelegate;
			handlerDelegate = () => behavior.HandleAsync(command, next, cancellationToken);
		}

		return await handlerDelegate();
	}

	public async Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
		where TQuery : IQuery<TResult>
	{
		var handler = _provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();

		var behaviors = _provider.GetServices<IPipelineBehavior<TQuery, TResult>>().Reverse();
		Func<Task<TResult>> handlerDelegate = () => handler.HandleAsync(query, cancellationToken);
		foreach (var behavior in behaviors)
		{
			var next = handlerDelegate;
			handlerDelegate = () => behavior.HandleAsync(query, next, cancellationToken);
		}

		return await handlerDelegate();
	}
}