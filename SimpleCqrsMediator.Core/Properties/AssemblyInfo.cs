using System.Runtime.CompilerServices;

// Make internals visible to the test project
[assembly: InternalsVisibleTo("SimpleCqrsMediator.Tests")]

// Make internals visible to Moq's dynamic proxy generation
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]