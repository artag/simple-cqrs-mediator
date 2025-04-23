using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Models;

namespace SimpleCqrsMediator.Api.Features.Todos.GetTodo;

public record GetTodoByIdQuery(long Id)
	: IQuery<TodoItemDto?>;