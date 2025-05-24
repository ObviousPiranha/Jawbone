using BenchmarkDotNet.Attributes;
using Jawbone.Stb;
using System.Runtime.InteropServices;

namespace Jawbone.Benchmark;

public static class DllImportPiranha
{
    [DllImport("PiranhaNative.dll", EntryPoint = "piranha_get_null", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint GetNull();
}

public static partial class LibraryImportPiranha
{
    [LibraryImport("PiranhaNative.dll", EntryPoint = "piranha_get_null")]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    public static partial nint GetNull();
}

[MemoryDiagnoser(false)]
public unsafe class NativeInteropBenchmark
{
    private static nint _fp;
    private readonly NativePiranha _nativePiranha;

    public NativeInteropBenchmark()
    {
        var handle = NativeLibrary.Load("./PiranhaNative.dll");
        _fp = NativeLibrary.GetExport(handle, "piranha_get_null");
        _nativePiranha = new(_ => _fp);
    }

    [Benchmark]
    public void SourceGenInterop()
    {
        _ = _nativePiranha.GetNull();
    }

    [Benchmark]
    public void StaticFp()
    {
        var f = (delegate* unmanaged[Cdecl]<nint>)_fp;
        _ = f();
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
