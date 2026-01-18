using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Tests.Models;

namespace SimpleCqrsMediator.Tests.Handlers;

/// <summary>
/// Test command handler implementation with a configurable handler function
/// </summary>
public class TestCommandHandler(Func<TestCommand, string> handler) : ICommandHandler<TestCommand, SimpleResponse>
{
	public Task<SimpleResponse> HandleAsync(TestCommand command, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(new SimpleResponse(handler(command)));
	}
}

/// <summary>
/// Test command handler with cancellation token support
/// </summary>
public class CancellableCommandHandler : ICommandHandler<TestCommand, SimpleResponse>
{
	public Task<SimpleResponse> HandleAsync(TestCommand command, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		return Task.FromResult(new SimpleResponse($"Command handled: {command.Input}"));
	}
}

/// <summary>
/// Test data command handler implementation with a configurable handler function
/// </summary>
public class TestDataCommandHandler(Func<TestDataCommand, TestData> handler) : ICommandHandler<TestDataCommand, TestData>
{
	public Task<TestData> HandleAsync(TestDataCommand command, CancellationToken cancellationToken)
	{
		return Task.FromResult(handler(command));
	}
}