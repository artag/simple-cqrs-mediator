using Microsoft.Extensions.Logging;

using Moq;

using SimpleCqrsMediator.Core.CQRS.Behaviors;
using SimpleCqrsMediator.Tests.Models;

namespace SimpleCqrsMediator.Tests;

[TestClass]
public class PipelineBehaviorTests
{
	[TestMethod]
	public async Task LoggingBehavior_ShouldCallNextDelegate()
	{
		// Arrange
		var loggerMock = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
		var behavior = new LoggingBehavior<TestRequest, TestResponse>(loggerMock.Object);

		bool delegateCalled = false;
		Func<Task<TestResponse>> next = () =>
		{
			delegateCalled = true;
			return Task.FromResult(new TestResponse("Success"));
		};

		// Act
		var result = await behavior.HandleAsync(new TestRequest("Test"), next, CancellationToken.None);

		// Assert
		Assert.IsTrue(delegateCalled, "The next delegate should be called");
		Assert.AreEqual("Success", result.Result);

		// Verify log calls with proper nullability handling
		loggerMock.Verify(
			l => l.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Starting: TestRequest")),
				It.IsAny<Exception>(),
				(Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
			Times.Once);

		loggerMock.Verify(
			l => l.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Completed: TestResponse")),
				It.IsAny<Exception>(),
				(Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
			Times.Once);
	}

	[TestMethod]
	public async Task LoggingBehavior_ShouldPreserveExceptions()
	{
		// Arrange
		var loggerMock = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
		var behavior = new LoggingBehavior<TestRequest, TestResponse>(loggerMock.Object);

		var expectedException = new InvalidOperationException("Test exception");
		Func<Task<TestResponse>> next = () => Task.FromException<TestResponse>(expectedException);

		// Act & Assert
		var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
			async () => await behavior.HandleAsync(new TestRequest("Test"), next, CancellationToken.None));

		Assert.AreEqual("Test exception", exception.Message);

		// Verify start log was called but not completion log with proper nullability handling
		loggerMock.Verify(
			l => l.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Starting: TestRequest")),
				It.IsAny<Exception>(),
				(Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
			Times.Once);
	}

	[TestMethod]
	public async Task LoggingBehavior_ShouldHandleCancellation()
	{
		// Arrange
		var loggerMock = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
		var behavior = new LoggingBehavior<TestRequest, TestResponse>(loggerMock.Object);

		using var cts = new CancellationTokenSource();
		cts.Cancel(); // Cancel immediately

		Func<Task<TestResponse>> next = () =>
		{
			cts.Token.ThrowIfCancellationRequested();
			return Task.FromResult(new TestResponse("Success"));
		};

		// Act & Assert
		await Assert.ThrowsExceptionAsync<OperationCanceledException>(
			async () => await behavior.HandleAsync(new TestRequest("Test"), next, cts.Token));

		// Verify start log was called but not completion log with proper nullability handling
		loggerMock.Verify(
			l => l.Log(
				It.IsAny<LogLevel>(),
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Starting: TestRequest")),
				It.IsAny<Exception>(),
				(Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
			Times.Once);
	}
}