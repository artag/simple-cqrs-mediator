using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Mappings;
using SimpleCqrsMediator.Core.Models;
using SimpleCqrsMediator.Core.Repositories;

namespace SimpleCqrsMediator.Api.Features.Todos.GetTodo;

public class GetTodoByIdQueryHandler(ITodoRepository todoRepository)
	: IQueryHandler<GetTodoByIdQuery, TodoItemDto?>
{
	public async Task<TodoItemDto?> HandleAsync(GetTodoByIdQuery query, CancellationToken cancellationToken)
	{
		var todo = await todoRepository.GetAsync(query.Id, cancellationToken);
		return todo?.ToDto();
	}
}