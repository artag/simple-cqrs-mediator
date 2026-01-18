using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Mappings;
using SimpleCqrsMediator.Core.Models;
using SimpleCqrsMediator.Core.Repositories;

namespace SimpleCqrsMediator.Api.Features.Todos.UpdateTodo;

public class UpdateTodoCommandHandler(ITodoRepository todoRepository)
	: ICommandHandler<UpdateTodoCommand, TodoItemDto>
{
	public async Task<TodoItemDto> HandleAsync(UpdateTodoCommand command, CancellationToken cancellationToken)
	{
		var existingTodo = await todoRepository.GetAsync(command.Id, cancellationToken);
		if (existingTodo == null)
		{
			throw new KeyNotFoundException($"Todo with ID {command.Id} was not found.");
		}

		existingTodo.Update(command.Title, command.Description, !existingTodo.IsCompleted);

		var updatedTodo = await todoRepository.UpdateAsync(existingTodo, cancellationToken);
		return updatedTodo.ToDto();
	}
}