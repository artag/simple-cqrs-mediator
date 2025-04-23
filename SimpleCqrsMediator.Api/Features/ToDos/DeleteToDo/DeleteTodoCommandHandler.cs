using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Repositories;

namespace SimpleCqrsMediator.Api.Features.Todos.DeleteTodo;

public class DeleteTodoCommandHandler(ITodoRepository todoRepository)
	: ICommandHandler<DeleteTodoCommand, bool>
{
	public async Task<bool> HandleAsync(DeleteTodoCommand command, CancellationToken cancellationToken)
	{
		return await todoRepository.DeleteAsync(command.Id, cancellationToken);
	}
}