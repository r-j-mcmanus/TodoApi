using Microsoft.EntityFrameworkCore;


public static class UserApi
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/register/", CreateUser).RequireCors(CorsPolicies.MyAllowSpecificOrigins);
    }

    static async Task<IResult> CreateUser(User user, UsersDb db)
    {

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/register/{user.Id}", user);
    }
}