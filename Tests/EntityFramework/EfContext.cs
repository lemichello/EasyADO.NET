using Microsoft.EntityFrameworkCore;
using Tests.EntityFramework.Entities;

namespace Tests.EntityFramework
{
    public class EfContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<EmptyTable> EmptyTables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=(LocalDb)\\MSSQLLocalDB;" +
                                        "initial catalog=EasyAdoNetTest;integrated security=True;" +
                                        "MultipleActiveResultSets=True;App=EntityFramework");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var admin = new Person {Id = 1, Name = "Maksym", Surname = "Lemich"};

            modelBuilder.Entity<Person>().HasData(
                admin,
                new Person {Id = 2, Name = "Angelina", Surname = "Ukrainets"},
                new Person {Id = 3, Name = "Dasha", Surname    = "Shevchuk"});

            modelBuilder.Entity<Role>().HasData(new Role {Id = 1, Name = "Admin", PersonId = 1});
        }
    }
}