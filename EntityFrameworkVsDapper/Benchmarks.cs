using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Dapper;
using EntityFrameworkVsDapper.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

public class Benchmarks
{
    private const int NumCustomers = 100;
    private const int OrdersPerCustomer = 10;

    private static readonly string ConnectionString = "Host=23.101.154.226;port=5432;Database=EfDapper;Username=postgres;Password=Az123456789@0;Include Error Detail=true";

    [GlobalSetup]
    public void Setup()
    {
        using var context = new AppDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed data
        var customers = new List<Customer>();

        for (int i = 0; i < NumCustomers; i++)
        {
            var customer = new Customer { Name = $"Customer {i}" };
            context.Customers.Add(customer);
            customers.Add(customer);
        }

        context.SaveChanges();

        foreach (var customer in customers)
        {
            for (int j = 0; j < OrdersPerCustomer; j++)
            {
                context.Orders.Add(new Order
                {
                    CustomerId = customer.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    Amount = (decimal)(j + 1) * 10
                });
            }
        }

        context.SaveChanges();
    }

    [Benchmark]
    public async Task<List<Customer>> EfCoreQuery()
    {
        using var context = new AppDbContext();
        return await context.Customers.Include(c => c.Orders).ToListAsync();
    }

    [Benchmark]
    public async Task<List<Customer>> DapperQuery()
    {
        try
        {
            using var connection = new NpgsqlConnection(ConnectionString);

            // Fetch customers
            var customers = (await connection.QueryAsync<Customer>("SELECT * FROM \"Customers\"")).ToList();
            Console.WriteLine($"Fetched {customers.Count} customers");

            var customerIds = customers.Select(c => c.CustomerId).ToList();

            // Fetch orders for the fetched customers
            var orders = (await connection.QueryAsync<Order>("SELECT * FROM \"Orders\" WHERE \"CustomerId\" = ANY(@CustomerIds)", new { CustomerIds = customerIds })).ToList();
            Console.WriteLine($"Fetched {orders.Count} orders");

            // Link orders to customers
            foreach (var customer in customers)
            {
                customer.Orders = orders.Where(o => o.CustomerId == customer.CustomerId).ToList();
            }

            return customers;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DapperQuery: {ex.Message}");
            throw;
        }
    }

}
