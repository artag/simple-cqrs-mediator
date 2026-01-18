using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Mappings;
using SimpleCqrsMediator.Core.Models;
using SimpleCqrsMediator.Core.Repositories;

namespace SimpleCqrsMediator.Api.Features.Todos.ToggleTodoCompletion;

public class ToggleTodoCompletionCommandHandler(ITodoRepository todoRepository)
	: ICommandHandler<ToggleTodoCompletionCommand, TodoItemDto>
{
	public async Task<TodoItemDto> HandleAsync(ToggleTodoCompletionCommand command, CancellationToken cancellationToken)
	{
		var toggledTodo = await todoRepository.ToggleCompletionAsync(command.Id, cancellationToken);
		return toggledTodo.ToDto();
	}
}