using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Word4Everyone.Model;

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
