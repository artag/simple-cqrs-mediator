using SimpleCqrsMediator.Core.Domain;
using SimpleCqrsMediator.Core.Models;

namespace SimpleCqrsMediator.Core.Mappings;

public static class TodoItemMappingExtensions
{
	public static TodoItemDto ToDto(this TodoItem item)
	{
		return new TodoItemDto(
			item.Id,
			item.Title,
			item.Description,
			item.IsCompleted,
			item.CreatedAt,
			item.CompletedAt);
	}
}