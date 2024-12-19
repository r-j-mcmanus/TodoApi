
public class User // public means that we can access this class from other parts of the app
{
    public Guid Id { get; set; }

    required public string? Email { get; set; }

    required public string UserName { get; set; }

    required public string HashedPassword { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}