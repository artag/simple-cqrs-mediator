using SimpleCqrsMediator.Core.CQRS;

namespace SimpleCqrsMediator.Tests.Models;

/// <summary>
/// Simple query model for testing query handling
/// </summary>
public record TestQuery(string Input) : IQuery<SimpleResponse>;

/// <summary>
/// Query model for testing data retrieval
/// </summary>
public record TestDataQuery(string Input) : IQuery<TestData>;