using System;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkVsDapper.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=23.101.154.226;port=5432;Database=EfDapper;Username=postgres;Password=Az123456789@0;");
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Seed data for Customers
        //    modelBuilder.Entity<Customer>().HasData(
        //        new Customer { CustomerId = 1, Name = "John Doe" },
        //        new Customer { CustomerId = 2, Name = "Jane Smith" },
        //        new Customer { CustomerId = 3, Name = "Bob Johnson" }
        //    );

        //    // Seed data for Orders
        //    modelBuilder.Entity<Order>().HasData(
        //        new Order { OrderId = 1, ProductName = "Laptop", Quantity = 1, CustomerId = 1 },
        //        new Order { OrderId = 2, ProductName = "Smartphone", Quantity = 2, CustomerId = 1 },
        //        new Order { OrderId = 3, ProductName = "Tablet", Quantity = 1, CustomerId = 2 },
        //        new Order { OrderId = 4, ProductName = "Monitor", Quantity = 2, CustomerId = 3 }
        //    );
        //}
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Amount { get; set; }

        public Customer Customer { get; set; }
    }
}

