using BenchmarkDotNet.Running;
using Jawbone.Benchmark;

// var summary = BenchmarkRunner.Run<AesBenchmark>();
// var summary = BenchmarkRunner.Run<NativeInteropBenchmark>();

var summary = BenchmarkRunner.Run<RopeStreamBenchmark>();
// var summary = BenchmarkRunner.Run<RopeStreamReadBenchmark>();
