using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Models;

namespace SimpleCqrsMediator.Api.Features.Todos.CreateTodo;

public record CreateTodoCommand(string Title, string? Description)
	: ICommand<TodoItemDto>;