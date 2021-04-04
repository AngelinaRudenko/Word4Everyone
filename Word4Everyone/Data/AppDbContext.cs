using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Word4Everyone.Model;

namespace Word4Everyone.Data
{
    public class AppDbContext : DbContext
    {
        //Tools -> NuGet Packacge Manager -> Package Manager Console
        //Add-Migration "label"
        //Update-Database

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
