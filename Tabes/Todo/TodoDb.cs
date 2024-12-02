using Microsoft.EntityFrameworkCore;
//This statement brings the Entity Framework Core library into scope, 
//allowing you to use EF Core-related classes like DbContext, DbSet, 
//and others. The Microsoft.EntityFrameworkCore namespace contains all 
//the necessary functionality for interacting with a database through EF Core.


class TodoDb : DbContext // inherits from the DbContext class, which is the base class provided by EF Core to manage the interaction with the database
{
    // DbContextOptions generic class:  contains configuration options for setting up the TodoDb
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { } // calls the constructor of the parent class (DbContext) with the options parameter, passing the database configuration to the base class.

    public DbSet<Todo> Todos => Set<Todo>(); // The arrow syntax (=>) is a shorthand for creating a read-only property (similar to get but more concise). So, Todos is a property that returns the DbSet<Todo> when accessed.
}