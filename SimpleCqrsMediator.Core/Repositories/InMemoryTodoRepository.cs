using SimpleCqrsMediator.Core.Domain;

namespace SimpleCqrsMediator.Core.Repositories;

public class InMemoryTodoRepository : ITodoRepository
{
	private readonly List<TodoItem> _todos = new();
	private long _nextId = 1;

	public Task<TodoItem> CreateAsync(TodoItem entity, CancellationToken cancellationToken)
	{
		entity.Update(_nextId++);
		_todos.Add(entity);
		return Task.FromResult(entity);
	}

	public Task<TodoItem?> GetAsync(long id, CancellationToken cancellationToken)
	{
		return Task.FromResult(_todos.FirstOrDefault(t => t.Id == id));
	}

	public Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken)
	{
		return Task.FromResult(_todos.AsEnumerable());
	}

	public Task<TodoItem> UpdateAsync(TodoItem entity, CancellationToken cancellationToken)
	{
		return Task.FromResult(_todos.First(t => t.Id == entity.Id));
	}

	public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
	{
		var existingEntity = _todos.FirstOrDefault(t => t.Id == id);
		if (existingEntity == null)
		{
			return Task.FromResult(false);
		}

		_todos.Remove(existingEntity);
		return Task.FromResult(true);
	}

	public async Task<TodoItem> ToggleCompletionAsync(long id, CancellationToken cancellationToken)
	{
		var existingEntity = await GetAsync(id, cancellationToken);
		if (existingEntity == null)
		{
			throw new KeyNotFoundException($"Todo with ID {id} not found.");
		}

		existingEntity.Update(!existingEntity.IsCompleted);
		return existingEntity;
	}
}