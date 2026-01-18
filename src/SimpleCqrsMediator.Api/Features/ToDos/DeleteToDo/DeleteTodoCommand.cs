using SimpleCqrsMediator.Core.CQRS;

namespace SimpleCqrsMediator.Api.Features.Todos.DeleteTodo;

public record DeleteTodoCommand(long Id)
	: ICommand<bool>;