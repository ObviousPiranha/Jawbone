using BenchmarkDotNet.Attributes;
using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;

namespace Piranha.Jawbone.Benchmark;

[MemoryDiagnoser(false)]
public class RopeStreamReadBenchmark
{
    private readonly byte[] _block = new byte[9000 * 100];
    private readonly MemoryStream _memoryStream = new();
    private readonly RopeStream _ropeStream = new();

    [GlobalSetup]
    public void SetUp()
    {
        RandomNumberGenerator.Fill(_block);
        _memoryStream.Write(_block);
        _ropeStream.Write(_block);
    }

    private void ReadStream(Stream stream)
    {
        stream.Position = 0;
        var n = stream.Read(_block);
        if (n != _block.Length)
            Throw();
    }

    [Benchmark(Baseline = true)]
    public void ReadMemoryStream()
    {
        ReadStream(_memoryStream);
    }

    [Benchmark]
    public void ReadRopeStream()
    {
        ReadStream(_ropeStream);
    }

    private static void Throw()
    {
        throw new InvalidOperationException();
    }
}
