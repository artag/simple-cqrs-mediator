namespace SimpleCqrsMediator.Tests.Models;

/// <summary>
/// Test request model for pipeline behaviors
/// </summary>
public record TestRequest(string Input);

/// <summary>
/// Test response model for pipeline behaviors
/// </summary>
public record TestResponse(string Result);