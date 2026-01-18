using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Mappings;
using SimpleCqrsMediator.Core.Models;
using SimpleCqrsMediator.Core.Repositories;

namespace SimpleCqrsMediator.Api.Features.Todos.GetAllTodos;

public class GetAllTodosQueryHandler(ITodoRepository todoRepository)
	: IQueryHandler<GetAllTodosQuery, IEnumerable<TodoItemDto>>
{
	public async Task<IEnumerable<TodoItemDto>> HandleAsync(GetAllTodosQuery query, CancellationToken cancellationToken)
	{
		var todos = await todoRepository.GetAllAsync(cancellationToken);
		return todos.Select(t => t.ToDto());
	}
}