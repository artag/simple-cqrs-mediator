using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.CQRS.Behaviors;
using SimpleCqrsMediator.Tests.Handlers;
using SimpleCqrsMediator.Tests.Models;

namespace SimpleCqrsMediator.Tests;

[TestClass]
public class MediatorIntegrationTests
{
	// Track execution order
	private readonly List<string> _executionSteps = new();

	[TestMethod]
	public async Task Mediator_ExecutesBehaviorsInCorrectOrder()
	{
		// Arrange
		var services = new ServiceCollection();

		// Add the mediator
		services.AddSingleton<IMediator, Core.CQRS.Mediator>();

		// Add command handler
		services.AddTransient<ICommandHandler<TestDataCommand, TestData>>(sp =>
			new TestDataCommandHandler(cmd =>
			{
				_executionSteps.Add("Handler executed");
				return new TestData($"Result: {cmd.Input}");
			}));

		// Add pipeline behaviors in specific order
		services.AddTransient<IPipelineBehavior<TestDataCommand, TestData>>(sp =>
			new FirstTestBehavior<TestDataCommand, TestData>(_executionSteps));
		services.AddTransient<IPipelineBehavior<TestDataCommand, TestData>>(sp =>
			new SecondTestBehavior(_executionSteps));

		var provider = services.BuildServiceProvider();
		var mediator = provider.GetRequiredService<IMediator>();
		_executionSteps.Clear();

		// Act
		var result = await mediator.SendCommandAsync<TestDataCommand, TestData>(
			new TestDataCommand("test-pipeline"), CancellationToken.None);

		// Assert
		Assert.AreEqual("Result: test-pipeline", result.Value);

		// Verify execution order
		CollectionAssert.AreEqual(
			new List<string>
			{
				"First behavior before",
				"Second behavior before",
				"Handler executed",
				"Second behavior after",
				"First behavior after"
			},
			_executionSteps);
	}

	[TestMethod]
	public async Task Mediator_ExecutesPipelineBehaviorsWithQueries()
	{
		// Arrange
		var services = new ServiceCollection();

		// Add the mediator
		services.AddSingleton<IMediator, Core.CQRS.Mediator>();

		// Add query handler
		services.AddTransient<IQueryHandler<TestDataQuery, TestData>>(sp =>
			new TestDataQueryHandler(query =>
			{
				_executionSteps.Add("Query handler executed");
				return new TestData($"Query: {query.Input}");
			}));

		// Add pipeline behaviors in specific order
		services.AddTransient<IPipelineBehavior<TestDataQuery, TestData>>(sp =>
			new FirstTestBehavior<TestDataQuery, TestData>(_executionSteps));

		var provider = services.BuildServiceProvider();
		var mediator = provider.GetRequiredService<IMediator>();
		_executionSteps.Clear();

		// Act
		var result = await mediator.SendQueryAsync<TestDataQuery, TestData>(
			new TestDataQuery("test-query"), CancellationToken.None);

		// Assert
		Assert.AreEqual("Query: test-query", result.Value);

		// Verify execution order
		CollectionAssert.AreEqual(
			new List<string>
			{
				"First behavior before",
				"Query handler executed",
				"First behavior after"
			},
			_executionSteps);
	}

	[TestMethod]
	public async Task Mediator_HandlesLoggingBehaviorCorrectly()
	{
		// Arrange
		var services = new ServiceCollection();
		var loggerMock = new Mock<ILogger<LoggingBehavior<TestDataCommand, TestData>>>();

		// Add the mediator
		services.AddSingleton<IMediator, Core.CQRS.Mediator>();

		// Add command handler
		services.AddTransient<ICommandHandler<TestDataCommand, TestData>>(sp =>
			new TestDataCommandHandler(_ => new TestData("Test result")));

		// Add logging behavior
		services.AddTransient<IPipelineBehavior<TestDataCommand, TestData>>(sp =>
			new LoggingBehavior<TestDataCommand, TestData>(loggerMock.Object));

		var provider = services.BuildServiceProvider();
		var mediator = provider.GetRequiredService<IMediator>();

		// Act
		var result = await mediator.SendCommandAsync<TestDataCommand, TestData>(
			new TestDataCommand("logging-test"), CancellationToken.None);

		// Assert
		Assert.AreEqual("Test result", result.Value);

		// Verify logging
		loggerMock.Verify(
			l => l.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Starting: TestDataCommand")),
				It.IsAny<Exception>(),
				(Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
			Times.Once);

		loggerMock.Verify(
			l => l.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Completed: TestData")),
				It.IsAny<Exception>(),
				(Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
			Times.Once);
	}

	#region Helper classes for testing

	private class FirstTestBehavior<TInput, TOutput> : IPipelineBehavior<TInput, TOutput>
		where TOutput : TestData
	{
		private readonly List<string> _steps;

		public FirstTestBehavior(List<string> steps)
		{
			_steps = steps;
		}

		public async Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken)
		{
			_steps.Add("First behavior before");
			var result = await next();
			_steps.Add("First behavior after");
			return result;
		}
	}

	private class SecondTestBehavior : IPipelineBehavior<TestDataCommand, TestData>
	{
		private readonly List<string> _steps;

		public SecondTestBehavior(List<string> steps)
		{
			_steps = steps;
		}

		public async Task<TestData> HandleAsync(TestDataCommand input, Func<Task<TestData>> next, CancellationToken cancellationToken)
		{
			_steps.Add("Second behavior before");
			var result = await next();
			_steps.Add("Second behavior after");
			return result;
		}
	}

	#endregion
}