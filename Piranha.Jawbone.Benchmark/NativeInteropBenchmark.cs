using BenchmarkDotNet.Attributes;
using Piranha.Jawbone.Stb;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Benchmark;

public static class DllImportPiranha
{
    [DllImport("PiranhaNative.dll", EntryPoint = "piranha_get_null", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint GetNull();
}

public static partial class LibraryImportPiranha
{
    [LibraryImport("PiranhaNative.dll", EntryPoint = "piranha_get_null")]
    [UnmanagedCallConv(CallConvs = new System.Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial nint GetNull();
}

[MemoryDiagnoser(false)]
public class NativeInteropBenchmark
{
    private readonly NativePiranha _nativePiranha;

    public NativeInteropBenchmark()
    {
        var handle = NativeLibrary.Load("./PiranhaNative.dll");
        _nativePiranha = new(_ => NativeLibrary.GetExport(handle, "piranha_get_null"));
    }

    [Benchmark]
    public void SourceGenInterop()
    {
        _ = _nativePiranha.GetNull();
    }

    [Benchmark(Baseline = true)]
    public void DllImportInterop()
    {
        _ = DllImportPiranha.GetNull();
    }

    [Benchmark]
    public void LibraryImportStbInterop()
    {
        _ = LibraryImportPiranha.GetNull();
    }
}
