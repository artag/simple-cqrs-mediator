using Microsoft.Extensions.DependencyInjection;

using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Tests.Handlers;
using SimpleCqrsMediator.Tests.Models;

namespace SimpleCqrsMediator.Tests;

[TestClass]
public class CommandAndQueryTests
{
	private readonly ServiceProvider _serviceProvider;

	public CommandAndQueryTests()
	{
		var services = new ServiceCollection();

		// Register mediator
		services.AddSingleton<IMediator, Core.CQRS.Mediator>();

		// Register command handlers
		services.AddTransient<ICommandHandler<TestCommand, SimpleResponse>>(sp =>
			new TestCommandHandler(cmd => $"Command executed: {cmd.Input}"));

		// Register query handlers
		services.AddTransient<IQueryHandler<TestQuery, SimpleResponse>>(sp =>
			new TestQueryHandler(query => $"Query executed: {query.Input}"));

		// Register data command/query handlers
		services.AddTransient<ICommandHandler<TestDataCommand, TestData>>(sp =>
			new TestDataCommandHandler(cmd => new TestData(cmd.Input)));

		services.AddTransient<IQueryHandler<TestDataQuery, TestData>>(sp =>
			new TestDataQueryHandler(query => new TestData(query.Input)));

		_serviceProvider = services.BuildServiceProvider();
	}

	[TestMethod]
	public async Task Mediator_ExecuteCommand_ShouldExecuteCommandHandler()
	{
		// Arrange
		var mediator = _serviceProvider.GetRequiredService<IMediator>();
		var command = new TestCommand("Test command");

		// Act
		var result = await mediator.SendCommandAsync<TestCommand, SimpleResponse>(command);

		// Assert
		Assert.IsNotNull(result);
		Assert.IsTrue(result.Result == "Command executed: Test command");
	}

	[TestMethod]
	public async Task Mediator_ExecuteCommand_WithCancellationToken_ShouldUseToken()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton<IMediator, Core.CQRS.Mediator>();

		// Register a command handler that will check for cancellation
		services.AddTransient<ICommandHandler<TestCommand, SimpleResponse>>(sp =>
			new CancellableCommandHandler());

		var provider = services.BuildServiceProvider();
		var mediator = provider.GetRequiredService<IMediator>();
		var command = new TestCommand("Test command");

		using var cts = new CancellationTokenSource();
		cts.Cancel(); // Cancel immediately

		// Act & Assert
		await Assert.ThrowsExceptionAsync<OperationCanceledException>(
			async () => await mediator.SendCommandAsync<TestCommand, SimpleResponse>(command, cts.Token));
	}

	[TestMethod]
	public async Task Mediator_ExecuteQuery_ShouldExecuteQueryHandler()
	{
		// Arrange
		var mediator = _serviceProvider.GetRequiredService<IMediator>();
		var query = new TestQuery("Test query");

		// Act
		var result = await mediator.SendQueryAsync<TestQuery, SimpleResponse>(query);

		// Assert
		Assert.IsNotNull(result);
		Assert.IsTrue(result.Result == "Query executed: Test query");
	}

	[TestMethod]
	public async Task Mediator_ExecuteQuery_WithCancellationToken_ShouldUseToken()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton<IMediator, Core.CQRS.Mediator>();

		// Register a query handler that will check for cancellation
		services.AddTransient<IQueryHandler<TestQuery, SimpleResponse>>(sp =>
			new CancellableQueryHandler());

		var provider = services.BuildServiceProvider();
		var mediator = provider.GetRequiredService<IMediator>();
		var query = new TestQuery("Test query");

		using var cts = new CancellationTokenSource();
		cts.Cancel(); // Cancel immediately

		// Act & Assert
		await Assert.ThrowsExceptionAsync<OperationCanceledException>(
			async () => await mediator.SendQueryAsync<TestQuery, SimpleResponse>(query, cts.Token));
	}

	[TestMethod]
	public async Task Mediator_ExecuteCommand_WithData_ShouldReturnData()
	{
		// Arrange
		var mediator = _serviceProvider.GetRequiredService<IMediator>();
		var command = new TestDataCommand("Test data");

		// Act
		var result = await mediator.SendCommandAsync<TestDataCommand, TestData>(command);

		// Assert
		Assert.IsNotNull(result);
		Assert.IsTrue(result.Value == "Test data");
	}

	[TestMethod]
	public async Task Mediator_ExecuteQuery_WithData_ShouldReturnData()
	{
		// Arrange
		var mediator = _serviceProvider.GetRequiredService<IMediator>();
		var query = new TestDataQuery("Test data");

		// Act
		var result = await mediator.SendQueryAsync<TestDataQuery, TestData>(query);

		// Assert
		Assert.IsNotNull(result);
		Assert.IsTrue(result.Value == "Test data");
	}
}