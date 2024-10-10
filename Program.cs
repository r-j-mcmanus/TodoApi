using Microsoft.EntityFrameworkCore;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // The policy name is arbitrary

// specify features of out app
var builder = WebApplication.CreateBuilder(args);
// The following highlighted code adds the database context to the dependency 
// injection (DI) container and enables displaying database-related exceptions:
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("ToDoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer(); 
// Enables the API Explorer, which is a service that provides metadata about the HTTP API. 
// The API Explorer is used by Swagger to generate the Swagger document.
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});

// we can set up multiple policies so different sites could use different endpoints
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://example.com",
                                              "http://www.otherExample.com");
                      });
});


var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins); // lett all endpoints use this policy

///////////////////////

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/", () => "Hello World!").RequireCors(MyAllowSpecificOrigins);

app.MapGet("/todoitems", async (TodoDb db) => await db.Todos.Select(x => new TodoDTO(x)).ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDb db) => await db.Todos.Select(x => new TodoDTO(x)).Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) => (
    await db.Todos.FindAsync(id)
        is Todo todo ? 
            Results.Ok(new TodoDTO(todo)) : 
            Results.NotFound()
));

// adds a new item to out df
app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

// can only put over items that already exist
app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
