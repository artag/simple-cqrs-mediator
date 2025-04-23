namespace SimpleCqrsMediator.Core.Models;

public record TodoItemDto(long? Id, string Title, string Description, bool IsCompleted, DateTime CreatedAt, DateTime? CompletedAt);