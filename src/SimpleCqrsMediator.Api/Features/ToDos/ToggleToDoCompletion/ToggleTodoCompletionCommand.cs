using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Models;

namespace SimpleCqrsMediator.Api.Features.Todos.ToggleTodoCompletion;

public record ToggleTodoCompletionCommand(long Id)
	: ICommand<TodoItemDto>;