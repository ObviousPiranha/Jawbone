using BenchmarkDotNet.Attributes;
using System.IO;
using System.Security.Cryptography;

namespace Piranha.Jawbone.Benchmark;

[MemoryDiagnoser(false)]
public class RopeStreamBenchmark
{
    private readonly byte[] _block = new byte[9000];

    [GlobalSetup]
    public void SetUp()
    {
        RandomNumberGenerator.Fill(_block);
    }

    private void FillStream(Stream stream)
    {
        for (int i = 0; i < 100; ++i)
            stream.Write(_block);
    }

    [Benchmark]
    public void FillRopeStream()
    {
        using var stream = new RopeStream();
        FillStream(stream);
    }

    [Benchmark(Baseline = true)]
    public void FillMemoryStream()
    {
        using var stream = new MemoryStream();
        FillStream(stream);
    }
}
