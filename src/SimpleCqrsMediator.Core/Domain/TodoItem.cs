namespace SimpleCqrsMediator.Core.Domain;

public class TodoItem
{
	public long Id { get; private set; }

	public string Title { get; private set; }

	public string Description { get; private set; }

	public bool IsCompleted { get; private set; } = false;

	public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

	public DateTime? CompletedAt { get; private set; }

	public TodoItem(string title, string description)
	{
		Title = title;
		Description = description;
	}

	public void Update(long id)
	{
		Id = id;
	}

	public void Update(bool isCompleted)
	{
		IsCompleted = isCompleted;
		CompletedAt = isCompleted ? DateTime.UtcNow : null;
	}

	public void Update(string title, string description, bool isCompleted)
	{
		Title = title;
		Description = description;
		Update(isCompleted);
	}
}