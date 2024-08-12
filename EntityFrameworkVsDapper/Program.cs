// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;


var summary = BenchmarkRunner.Run<EfCoreDapperBenchmark>();
//var summary =BenchmarkRunner.Run<Benchmarks>();



