using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Tests.Models;

namespace SimpleCqrsMediator.Tests.Handlers;

/// <summary>
/// Test query handler implementation with a configurable handler function
/// </summary>
public class TestQueryHandler(Func<TestQuery, string> handler) : IQueryHandler<TestQuery, SimpleResponse>
{
	public Task<SimpleResponse> HandleAsync(TestQuery query, CancellationToken cancellationToken = default)
	{
		return Task.FromResult(new SimpleResponse(handler(query)));
	}
}

/// <summary>
/// Test query handler with cancellation token support
/// </summary>
public class CancellableQueryHandler : IQueryHandler<TestQuery, SimpleResponse>
{
	public Task<SimpleResponse> HandleAsync(TestQuery query, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		return Task.FromResult(new SimpleResponse($"Query handled: {query.Input}"));
	}
}

/// <summary>
/// Test data query handler implementation with a configurable handler function
/// </summary>
public class TestDataQueryHandler(Func<TestDataQuery, TestData> handler) : IQueryHandler<TestDataQuery, TestData>
{
	public Task<TestData> HandleAsync(TestDataQuery query, CancellationToken cancellationToken)
	{
		return Task.FromResult(handler(query));
	}
}