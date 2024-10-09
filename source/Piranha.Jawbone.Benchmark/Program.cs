using BenchmarkDotNet.Running;
using Piranha.Jawbone.Benchmark;
using Piranha.Jawbone.Net;

// var summary = BenchmarkRunner.Run<AesBenchmark>();
// var summary = BenchmarkRunner.Run<AddressKeyBenchmark<AddressV4>>();
// var summary = BenchmarkRunner.Run<AddressKeyBenchmark<AddressV6>>();
// var summary = BenchmarkRunner.Run<NativeInteropBenchmark>();
//var summary = BenchmarkRunner.Run<SocketBenchmark>();

// var summary = BenchmarkRunner.Run<RopeStreamBenchmark>();
// var summary = BenchmarkRunner.Run<RopeStreamReadBenchmark>();

var summary = BenchmarkRunner.Run<SocketSendReceiveBenchmark>();

// using var test = new SocketSendReceiveBenchmark();
// for (int i = 0; i < 10; ++i)
//     test.RunSocket();
// test.RunJawbone();
