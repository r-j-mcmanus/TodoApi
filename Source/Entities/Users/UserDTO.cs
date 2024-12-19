
// DTO = a Transfer Object, controls what from the DB is returned

public class UserDTO // public means that we can access this class from other parts of the app
{
    public UserDTO(User user)
    {
        (Id, Email, UserName) = (user.Id, user.Email, user.UserName);
    }

    public Guid Id { get; set; } // automatically define the get and set methods for this property
    required public string? Email { get; set; } // the ? indicates that it can also be null, meaning the name is optional
    public string UserName { get; set; } // public means the property is accessible from anywhere, ie can call get set
}