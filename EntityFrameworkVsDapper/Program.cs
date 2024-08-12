// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args)
    {
        //var summary = BenchmarkRunner.Run<EfCoreDapperBenchmark>();
        var summary = new EfCoreDapperBenchmark();
       var dd = summary.GetAllCustomersDapper();
        //var ds =summary.GetCustomerByIdDapper();
    }
}

