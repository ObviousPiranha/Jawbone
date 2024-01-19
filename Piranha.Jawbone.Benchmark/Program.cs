using BenchmarkDotNet.Running;
using Piranha.Jawbone.Benchmark;
using Piranha.Jawbone.Net;

// var summary = BenchmarkRunner.Run<AesBenchmark>();
// var summary = BenchmarkRunner.Run<AddressKeyBenchmark<AddressV4>>();
// var summary = BenchmarkRunner.Run<AddressKeyBenchmark<AddressV6>>();
var summary = BenchmarkRunner.Run<NativeInteropBenchmark>();
// var summary = BenchmarkRunner.Run<SocketBenchmark>();
