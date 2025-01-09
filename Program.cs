using Microsoft.EntityFrameworkCore;
using DotNetEnv;

Env.Load();

// specify features of out app
var builder = WebApplication.CreateBuilder(args);
// The following highlighted code adds the database context to the dependency 
// injection (DI) container and enables displaying database-related exceptions:
builder.Services.AddDbContext<TodoDb>(opt => 
    opt.UseSqlite("Data Source=TodoList.db")
);
builder.Services.AddDbContext<UsersDb>(opt => 
    opt.UseSqlite("Data Source=Users.db")
);
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
    options.AddPolicy(name: CorsPolicies.MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5500")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});


var app = builder.Build();
app.UseCors(CorsPolicies.MyAllowSpecificOrigins);

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

app.MapGet("/", () => "Hello World!").RequireCors(CorsPolicies.MyAllowSpecificOrigins);

app.MapUserEndpoints();
app.MapTodoEndpoints();

app.Run();
