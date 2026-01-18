namespace SimpleCqrsMediator.Tests.Models;

/// <summary>
/// Simple response model for basic command and query operations
/// </summary>
public record SimpleResponse(string Result);

/// <summary>
/// Test data model for data-focused command and query operations
/// </summary>
public record TestData(string Value);