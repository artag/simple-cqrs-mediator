# Simple CQRS Mediator API

This project demonstrates a lightweight implementation of the CQRS pattern with a custom mediator implementation, as an alternative to using the MediatR library.

## Overview

Based on the article [The Easiest Way to Replace MediatR](https://medium.com/@paveluzunov/the-easiest-way-to-replace-mediatr-cb6a0fa07ded), this project implements:

- A custom mediator that supports commands and queries
- Clean separation of concerns following CQRS principles
- Simple handlers for each command and query
- A Todo API as a practical demonstration

## Project Structure

- **SimpleCqrsMediator.Core**:
  - Mediator, Command/Query interfaces
  - Domain models and DTOs
  - Mapping extensions
  - In-memory repositories
  - Pipeline behaviors

- **SimpleCqrsMediator.Api**:
  - Controllers with versioning
  - Feature-organized command and query handlers
  - HTTP requests file for testing

- **SimpleCqrsMediator.Tests**:
  - Unit tests for Mediator, Command/Query interfaces

## API Endpoints

The API implements RESTful endpoints for Todo management:

- `GET /api/v1/todos` - Retrieve all todos
- `GET /api/v1/todos/{id}` - Retrieve a specific todo
- `POST /api/v1/todos` - Create a new todo
- `PUT /api/v1/todos/{id}` - Update an existing todo
- `PATCH /api/v1/todos/{id}/toggle` - Toggle todo completion status
- `DELETE /api/v1/todos/{id}` - Delete a todo

## How to Test

Use the included HTTP file (Requests.http) to test the API endpoints.