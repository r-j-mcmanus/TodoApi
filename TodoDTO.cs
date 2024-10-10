
// DTO = a Transfer Object, controls what from the DB is returned

public class TodoDTO // public means that we can access this class from other parts of the app
{
    public TodoDTO() {}
    public TodoDTO(Todo todo) {
        (Id, Name, IsComplete) = (todo.Id, todo.Name, todo.IsComplete);
    }

    public int Id { get; set; } // automatically define the get and set methods for this property
    public string? Name { get; set; } // the ? indicates that it can also be null, meaning the name is optional
    public bool IsComplete { get; set; } // public means the property is accessible from anywhere, ie can call get set
}