using System;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Net;

namespace Piranha.Jawbone.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SocketBenchmark>();
        }
    }
}
