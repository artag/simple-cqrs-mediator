using Microsoft.Extensions.DependencyInjection;

using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.CQRS.Behaviors;
using SimpleCqrsMediator.Core.Repositories;

namespace SimpleCqrsMediator.Core.Extensions;

public static class WebApplicationBuilderExtensions
{
	public static IServiceCollection ConfigureCore(this IServiceCollection services)
	{
		services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();

		// Register CQRS mediator
		services.AddSingleton<IMediator, Mediator>();

		// Register pipeline behaviors
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}