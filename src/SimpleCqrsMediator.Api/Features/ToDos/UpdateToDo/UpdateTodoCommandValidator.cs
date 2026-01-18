using FluentValidation;

namespace SimpleCqrsMediator.Api.Features.Todos.UpdateTodo;

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
	public UpdateTodoCommandValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0).WithMessage("ID must be greater than 0");

		RuleFor(x => x.Title)
			.NotEmpty().WithMessage("Title is required")
			.MinimumLength(3).WithMessage("Title must be at least 3 characters")
			.MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

		RuleFor(x => x.Description)
			.MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
	}
}