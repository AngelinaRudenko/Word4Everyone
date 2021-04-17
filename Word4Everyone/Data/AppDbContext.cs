using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Word4Everyone.Data
{
    public class AppDbContext : IdentityDbContext//<User, IdentityRole<int>, int>
    {
        //Tools -> NuGet Packacge Manager -> Package Manager Console
        //Add-Migration "label"
        //Update-Database

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();   //delete Db with an old schema
            //Database.EnsureCreated();   //create Db with a new Schema
        }

        //public DbSet<User> Users { get; set; }
    }
}
