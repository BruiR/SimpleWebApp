using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Domain.Models;
using System.Reflection;

namespace SimpleWebApp.Repository
{
    public class AppDbContext : DbContext
    {
        public DbSet<AuthorizedPerson> AuthorizedPersons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(AppDbContext)));
            //modelBuilder.Entity<AuthorizedPerson>().HasData(
            //    new AuthorizedPerson[]
            //    {
            //        new AuthorizedPerson { Id =1, Login = "admin", Password = "admin", Role = "admin" },
            //        new AuthorizedPerson { Id =2, Login = "string", Password = "string", Role = "admin" }
            //    });
            //modelBuilder.Entity<Role>().HasData(
            //    new Role[]
            //    {
            //        new Role { Id =1, Name ="User"},
            //        new Role { Id =2, Name ="Admin"},
            //        new Role { Id =3, Name ="Support"},
            //        new Role { Id =4, Name ="SuperAdmin"},

            //    });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name = DefaultConnection");
            }
        }
    }
}
