using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using SimpleCqrsMediator.Api.Features.Todos.CreateTodo;
using SimpleCqrsMediator.Api.Features.Todos.DeleteTodo;
using SimpleCqrsMediator.Api.Features.Todos.GetAllTodos;
using SimpleCqrsMediator.Api.Features.Todos.GetTodo;
using SimpleCqrsMediator.Api.Features.Todos.ToggleTodoCompletion;
using SimpleCqrsMediator.Api.Features.Todos.UpdateTodo;
using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Models;

namespace SimpleCqrsMediator.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/todos")]
public class TodosController(IMediator mediator)
	: ControllerBase
{
	private readonly IMediator _mediator = mediator;

	// GET: api/v1/todos
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodos(CancellationToken cancellationToken)
	{
		var query = new GetAllTodosQuery();
		var result = await _mediator.SendQueryAsync<GetAllTodosQuery, IEnumerable<TodoItemDto>>(query, cancellationToken);
		return Ok(result);
	}

	// GET: api/v1/todos/{id}
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<TodoItemDto>> GetTodo(long id, CancellationToken cancellationToken)
	{
		var query = new GetTodoByIdQuery(id);
		var result = await _mediator.SendQueryAsync<GetTodoByIdQuery, TodoItemDto?>(query, cancellationToken);

		if (result == null)
		{
			return NotFound();
		}

		return Ok(result);
	}

	// POST: api/v1/todos
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<TodoItemDto>> CreateTodo([FromBody] CreateTodoCommand command, CancellationToken cancellationToken)
	{
		try
		{
			var result = await _mediator.SendCommandAsync<CreateTodoCommand, TodoItemDto>(command, cancellationToken);
			return CreatedAtAction(nameof(GetTodo), new { id = result.Id, version = "1.0" }, result);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(ex.Message);
		}
		catch (ValidationException ex)
		{
			return BadRequest(ex.Errors);
		}
	}

	// PUT: api/v1/todos/{id}
	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<TodoItemDto>> UpdateTodo(long id, [FromBody] UpdateTodoCommand command, CancellationToken cancellationToken)
	{
		try
		{
			// Override the ID from the route
			command = command with { Id = id };

			var result = await _mediator.SendCommandAsync<UpdateTodoCommand, TodoItemDto>(command, cancellationToken);
			return Ok(result);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
		catch (ArgumentException ex)
		{
			return BadRequest(ex.Message);
		}
		catch (ValidationException ex)
		{
			return BadRequest(ex.Errors);
		}
	}

	// DELETE: api/v1/todos/{id}
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteTodo(long id, CancellationToken cancellationToken)
	{
		var command = new DeleteTodoCommand(id);
		var result = await _mediator.SendCommandAsync<DeleteTodoCommand, bool>(command, cancellationToken);

		if (!result)
		{
			return NotFound();
		}

		return NoContent();
	}

	// PATCH: api/v1/todos/{id}/toggle
	[HttpPatch("{id}/toggle")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<TodoItemDto>> ToggleTodoCompletion(long id, CancellationToken cancellationToken)
	{
		try
		{
			var command = new ToggleTodoCompletionCommand(id);
			var result = await _mediator.SendCommandAsync<ToggleTodoCompletionCommand, TodoItemDto>(command, cancellationToken);
			return Ok(result);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}
}