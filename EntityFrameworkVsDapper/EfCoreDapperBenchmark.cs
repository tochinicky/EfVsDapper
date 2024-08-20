using BenchmarkDotNet.Attributes;
using EntityFrameworkVsDapper.Data;
using EntityFrameworkVsDapper.Repositories;
using System.Threading.Tasks;

[MemoryDiagnoser]
public class EfCoreDapperBenchmark
{
    private readonly EfCoreCustomerRepository _efCoreRepository;
    private readonly DapperCustomerRepository _dapperRepository;
    private const int NumCustomers = 100;
    private const int OrdersPerCustomer = 10;
    public EfCoreDapperBenchmark()
    {
        var connectionString = "Host=23.101.154.226;port=5432;Database=EfDapper;Username=postgres;Password=Az123456789@0;Include Error Detail=true";
        var dbContext = new AppDbContext();
        _efCoreRepository = new EfCoreCustomerRepository(dbContext);
        _dapperRepository = new DapperCustomerRepository(connectionString);
    }

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
    public async Task GetAllCustomersEfCore()
    {
        await _efCoreRepository.GetCustomersAsync();
    }

    [Benchmark]
    public async Task GetAllCustomersDapper()
    {
        await _dapperRepository.GetCustomersAsync();
    }

    [Benchmark]
    public async Task GetCustomerByIdEfCore()
    {
        await _efCoreRepository.GetCustomerByIdAsync(1);
    }

    [Benchmark]
    public async Task GetCustomerByIdDapper()
    {
        await _dapperRepository.GetCustomerByIdAsync(1);
    }
    [Benchmark]
    public async Task GetOrdersByCustomerIdEfCore()
    {
        await _efCoreRepository.GetOrdersByCustomerIdAsync(1);
    }
    [Benchmark]
    public async Task GetOrdersByCustomerIdDapper()
    {
        await _dapperRepository.GetOrdersByCistomerId(1);
    }
}
