using Microsoft.EntityFrameworkCore;


public static class UserApi
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/register/", CreateUser).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
        endpoints.MapPost("/login/", Login).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
        endpoints.MapPost("/ValidateJWT/", ValidateJWT).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
    }

    static async Task<IResult> CreateUser(LoginRequest loginReq, UsersDb db)
    {

        // Check if a user with the same UserName already exists
        var existingUser = await db.Users.FirstOrDefaultAsync(u => u.UserName == loginReq.UserName);

        if (existingUser != null)
        {
            return TypedResults.Conflict("A user with the same username already exists.");
        }

        string hashedPassword = PasswordHasher.HashPassword(loginReq.Password);
        
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = loginReq.UserName,
            HashedPassword = hashedPassword,
            Email = null, // Assuming email is not provided in LoginRequest
            CreatedOn = DateTime.UtcNow
        };

        db.Users.Add(newUser);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/register/{newUser.Id}", newUser);
    }

    static async Task<IResult> Login(LoginRequest loginReq, UsersDb db)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == loginReq.UserName);

        if (user == null)
        {
            // User not found
            return TypedResults.Unauthorized();
        }

        string password = loginReq.Password;
        string hashed_password = user.HashedPassword;
        bool verified_password = PasswordHasher.VerifyPassword(password, hashed_password);


        if(verified_password){
            string JWT = JsonWebToken.makeToken();
            return TypedResults.Ok( new {Token = JWT} );
        }
        else{
            return TypedResults.Unauthorized();
        }
    }

    static async Task<IResult> ValidateJWT(string jwt)
    {
        if(JsonWebToken.validateToken(jwt)){
            return TypedResults.Ok();
        }
        else{
            return TypedResults.Unauthorized();
        }
    }
}