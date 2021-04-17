using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Word4Everyone.Model;

namespace Word4Everyone.Data
{
    public class AppDbContext : IdentityDbContext//<User, IdentityRole<int>, int>
    {
        //Tools -> NuGet Package Manager -> Package Manager Console
        //Add-Migration "label"
        //Update-Database

        //public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();   // Удаляем бд со старой схемой
            Database.EnsureCreated();   // Cоздаем БД при первом обращении
        }
    }
}
