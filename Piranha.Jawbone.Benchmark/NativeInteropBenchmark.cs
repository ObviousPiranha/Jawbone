using BenchmarkDotNet.Attributes;
using Piranha.Jawbone.Stb;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Benchmark;

public static class DllImportStb
{
    [DllImport("PiranhaNative.dll", EntryPoint = "piranha_get_string", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint GetString();
}

public static partial class LibraryImportStb
{
    [LibraryImport("PiranhaNative.dll", EntryPoint = "piranha_get_string")]
    [UnmanagedCallConv(CallConvs = new System.Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial nint GetString();
}

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

    [Benchmark]
    public void DllImportInterop()
    {
        var ptr = DllImportStb.GetString();
        _ = Marshal.PtrToStringUTF8(ptr);
    }

    [Benchmark]
    public void LibraryImportStbInterop()
    {
        var ptr = LibraryImportStb.GetString();
        _ = Marshal.PtrToStringUTF8(ptr);
    }
}
