using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Models;

namespace SimpleCqrsMediator.Api.Features.Todos.GetAllTodos;

public record GetAllTodosQuery()
	: IQuery<IEnumerable<TodoItemDto>>;