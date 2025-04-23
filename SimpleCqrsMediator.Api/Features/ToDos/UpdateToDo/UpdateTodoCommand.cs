using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Models;

namespace SimpleCqrsMediator.Api.Features.Todos.UpdateTodo;

public record UpdateTodoCommand(long Id, string Title, string? Description, bool IsCompleted)
	: ICommand<TodoItemDto>;