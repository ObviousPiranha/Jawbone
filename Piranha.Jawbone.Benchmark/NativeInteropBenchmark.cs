using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using Piranha.Jawbone.Stb;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Benchmark;

[MemoryDiagnoser(false)]
public class NativeInteropBenchmark
{
    private readonly IStb _stb;
    private readonly StbPointers _stbPointers;

    public NativeInteropBenchmark()
    {
        var library = NativeLibraryInterface.FromFile<IStb>("PiranhaNative.dll", StbExtensions.ResolveName);
        _stb = library.Library;

        var handle = NativeLibrary.Load("./PiranhaNative.dll");
        _stbPointers = new(handle);
    }

    [Benchmark(Baseline = true)]
    public void InterfaceInterop()
    {
        _ = _stb.PiranhaGetString();
    }

    [Benchmark]
    public void PointerInterop()
    {
        _ = _stbPointers.GetString();
    }
}