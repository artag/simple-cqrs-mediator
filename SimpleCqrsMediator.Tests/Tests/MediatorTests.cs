using Microsoft.Extensions.DependencyInjection;

using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Tests.Handlers;
using SimpleCqrsMediator.Tests.Models;

namespace SimpleCqrsMediator.Tests;

[TestClass]
public class MediatorTests
{
	[TestMethod]
	public async Task Mediator_SendsCommandToHandler()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton<IMediator, Core.CQRS.Mediator>();
		services.AddTransient<ICommandHandler<TestCommand, SimpleResponse>>(sp =>
			new TestCommandHandler(cmd => $"Handled: {cmd.Input}"));

		var provider = services.BuildServiceProvider();
		var mediator = provider.GetRequiredService<IMediator>();

		// Act
		var result = await mediator.SendCommandAsync<TestCommand, SimpleResponse>(new TestCommand("Test"));

		// Assert
		Assert.AreEqual("Handled: Test", result.Result);
	}

	[TestMethod]
	public async Task Mediator_SendsQueryToHandler()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton<IMediator, Core.CQRS.Mediator>();
		services.AddTransient<IQueryHandler<TestQuery, SimpleResponse>>(sp =>
			new TestQueryHandler(query => $"Result: {query.Input}"));

		var provider = services.BuildServiceProvider();
		var mediator = provider.GetRequiredService<IMediator>();

		// Act
		var result = await mediator.SendQueryAsync<TestQuery, SimpleResponse>(new TestQuery("Test"));

		// Assert
		Assert.AreEqual("Result: Test", result.Result);
	}

	[TestMethod]
	[ExpectedException(typeof(InvalidOperationException))]
	public async Task Mediator_ThrowsWhenHandlerNotFound()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton<IMediator, Core.CQRS.Mediator>();
		var provider = services.BuildServiceProvider();
		var mediator = provider.GetRequiredService<IMediator>();

		// Act - should throw
		await mediator.SendCommandAsync<TestCommand, SimpleResponse>(new TestCommand("Test"));
	}
}