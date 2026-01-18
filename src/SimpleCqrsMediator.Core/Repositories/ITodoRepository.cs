using SimpleCqrsMediator.Core.Domain;

namespace SimpleCqrsMediator.Core.Repositories;

public interface ITodoRepository
{
	Task<TodoItem> CreateAsync(TodoItem entity, CancellationToken cancellationToken = default);
	Task<TodoItem?> GetAsync(long id, CancellationToken cancellationToken = default);
	Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<TodoItem> UpdateAsync(TodoItem entity, CancellationToken cancellationToken = default);
	Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
	Task<TodoItem> ToggleCompletionAsync(long id, CancellationToken cancellationToken = default);
}