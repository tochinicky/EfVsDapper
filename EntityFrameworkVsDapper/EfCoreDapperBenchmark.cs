using BenchmarkDotNet.Attributes;
using EntityFrameworkVsDapper.Data;
using EntityFrameworkVsDapper.Repositories;
using System.Threading.Tasks;

public class EfCoreDapperBenchmark
{
    private readonly EfCoreCustomerRepository _efCoreRepository;
    private readonly DapperCustomerRepository _dapperRepository;

    public EfCoreDapperBenchmark()
    {
        var connectionString = "Host=23.101.154.226;port=5432;Database=EfDapper;Username=postgres;Password=Az123456789@0;";
        var dbContext = new AppDbContext();
        _efCoreRepository = new EfCoreCustomerRepository(dbContext);
        _dapperRepository = new DapperCustomerRepository(connectionString);
    }

    [Benchmark]
    public async Task GetAllCustomersEfCore()
    {
        await _efCoreRepository.GetCustomersAsync();
    }

    //[Benchmark]
    public async Task GetAllCustomersDapper()
    {
       var get =  await _dapperRepository.GetCustomersAsync();
        return;
    }

    [Benchmark]
    public async Task GetCustomerByIdEfCore()
    {
        await _efCoreRepository.GetCustomerByIdAsync(1);
    }

    //[Benchmark]
    public async Task GetCustomerByIdDapper()
    {
       var ff=  await _dapperRepository.GetCustomerByIdAsync(1);
        return;
    }
}
