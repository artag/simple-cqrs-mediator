using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Domain;
using SimpleCqrsMediator.Core.Mappings;
using SimpleCqrsMediator.Core.Models;
using SimpleCqrsMediator.Core.Repositories;

namespace SimpleCqrsMediator.Api.Features.Todos.CreateTodo;

public class CreateTodoCommandHandler(ITodoRepository todoRepository)
	: ICommandHandler<CreateTodoCommand, TodoItemDto>
{
	public async Task<TodoItemDto> HandleAsync(CreateTodoCommand command, CancellationToken cancellationToken)
	{
		var todo = new TodoItem(command.Title, command.Description);
		var createdTodo = await todoRepository.CreateAsync(todo, cancellationToken);
		return createdTodo.ToDto();
	}
}