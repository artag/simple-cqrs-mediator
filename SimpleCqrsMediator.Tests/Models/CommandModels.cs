using SimpleCqrsMediator.Core.CQRS;

namespace SimpleCqrsMediator.Tests.Models;

/// <summary>
/// Simple command model for testing command handling
/// </summary>
public record TestCommand(string Input) : ICommand<SimpleResponse>;

/// <summary>
/// Command model for testing data manipulation
/// </summary>
public record TestDataCommand(string Input) : ICommand<TestData>;