using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using SimpleCqrsMediator.Api.Features.Todos.CreateTodo;
using SimpleCqrsMediator.Api.Features.Todos.DeleteTodo;
using SimpleCqrsMediator.Api.Features.Todos.GetAllTodos;
using SimpleCqrsMediator.Api.Features.Todos.GetTodo;
using SimpleCqrsMediator.Api.Features.Todos.ToggleTodoCompletion;
using SimpleCqrsMediator.Api.Features.Todos.UpdateTodo;
using SimpleCqrsMediator.Api.Middlewares;
using SimpleCqrsMediator.Core.CQRS;
using SimpleCqrsMediator.Core.Extensions;
using SimpleCqrsMediator.Core.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCore();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});

// Register validators for commands
builder.Services.AddTransient<IValidator<CreateTodoCommand>, CreateTodoCommandValidator>();
builder.Services.AddTransient<IValidator<UpdateTodoCommand>, UpdateTodoCommandValidator>();

// Register command handlers
builder.Services.AddTransient<ICommandHandler<CreateTodoCommand, TodoItemDto>, CreateTodoCommandHandler>();
builder.Services.AddTransient<ICommandHandler<UpdateTodoCommand, TodoItemDto>, UpdateTodoCommandHandler>();
builder.Services.AddTransient<ICommandHandler<DeleteTodoCommand, bool>, DeleteTodoCommandHandler>();
builder.Services.AddTransient<ICommandHandler<ToggleTodoCompletionCommand, TodoItemDto>, ToggleTodoCompletionCommandHandler>();

// Register query handlers
builder.Services.AddTransient<IQueryHandler<GetTodoByIdQuery, TodoItemDto?>, GetTodoByIdQueryHandler>();
builder.Services.AddTransient<IQueryHandler<GetAllTodosQuery, IEnumerable<TodoItemDto>>, GetAllTodosQueryHandler>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();

// Add the error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();