using Microsoft.EntityFrameworkCore;


class UsersDb : DbContext 
{
    public UsersDb(DbContextOptions<UsersDb> options)
        : base(options) { }

    public DbSet<User> Users  => Set<User>();
}