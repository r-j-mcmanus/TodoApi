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

// CORS
// we can set up multiple policies so different sites could use different endpoints
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5500")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
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

///

// add new path in url
var todoItems = app.MapGroup("/todoitems");

app.MapGet("/", () => "Hello World!").RequireCors(MyAllowSpecificOrigins);

// use methods not lambdas
todoItems.MapGet("/", GetAllTodos).RequireCors(MyAllowSpecificOrigins);
todoItems.MapGet("/complete", GetCompleteTodos).RequireCors(MyAllowSpecificOrigins);
todoItems.MapGet("/{id}", GetTodo).RequireCors(MyAllowSpecificOrigins);
todoItems.MapPost("/", CreateTodo).RequireCors(MyAllowSpecificOrigins);
todoItems.MapPut("/{id}", UpdateTodo).RequireCors(MyAllowSpecificOrigins);
todoItems.MapDelete("/{id}", DeleteTodo).RequireCors(MyAllowSpecificOrigins);

app.Run();

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
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

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

///

app.Run();
