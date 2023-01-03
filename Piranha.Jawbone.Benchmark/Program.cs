using BenchmarkDotNet.Running;
using Piranha.Jawbone.Benchmark;
using Piranha.Jawbone.Net;

// var summary = BenchmarkRunner.Run<AesBenchmark>();
// var summary = BenchmarkRunner.Run<AddressKeyBenchmark<Address32>>();
// var summary = BenchmarkRunner.Run<AddressKeyBenchmark<Address128>>();
// var summary = BenchmarkRunner.Run<NativeInteropBenchmark>();
var summary = BenchmarkRunner.Run<SocketBenchmark>();
