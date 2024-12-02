
using Microsoft.EntityFrameworkCore;


public static class TodoCrud
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // use methods not lambdas
        // var todoItems = app.MapGroup("/todoitems"); !!!
        endpoints.MapGet("/todoitems/", GetAllTodos).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
        endpoints.MapGet("/todoitems/complete", GetCompleteTodos).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
        endpoints.MapGet("/todoitems/{id}", GetTodo).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
        endpoints.MapPost("/todoitems/", CreateTodo).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
        endpoints.MapPut("/todoitems/{id}", UpdateTodo).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
        endpoints.MapDelete("/todoitems/{id}", DeleteTodo).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
    }


    // static - The static keyword indicates that the method belongs to the class itself rather than an instance of the class
    // async - The async modifier is used to indicate that the method contains asynchronous operations. This means the method can perform non-blocking tasks, such as accessing a database or performing I/O operations.
    // Task - a Task represents an asynchronous operation that will eventually complete and return a result. The Task<IResult> means that this asynchronous method will eventually return a value of type IResult. It's like a promise that some value will be available in the future after the asynchronous work is done.
    // IResult - IResult is an interface from ASP.NET Core that represents the result of a web request (an HTTP response). 
    static async Task<IResult> GetAllTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.ToArrayAsync());
    }

    static async Task<IResult> GetCompleteTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());
    }

    static async Task<IResult> GetTodo(int id, TodoDb db)
    {
        return await db.Todos.FindAsync(id)
            is Todo todo
                ? TypedResults.Ok(todo)
                : TypedResults.NotFound();
    }

    static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
    {
        // System.Console.WriteLine($"adding to {todo.Id} {todo.Name} todo");
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        // System.Console.WriteLine($"added to {todo.Id} {todo.Name} todo");

        return TypedResults.Created($"/todoitems/{todo.Id}", todo);
    }

    static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = inputTodo.Name;
        todo.IsComplete = inputTodo.IsComplete;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteTodo(int id, TodoDb db)
    {
        if (await db.Todos.FindAsync(id) is Todo todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}