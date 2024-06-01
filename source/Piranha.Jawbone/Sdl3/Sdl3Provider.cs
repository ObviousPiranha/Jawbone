using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

sealed class Sdl3Provider : IDisposable
{
    private readonly nint _handle;

    public Sdl3Library Library { get; }

    public Sdl3Provider(string library, SdlInit flags)
    {
        _handle = NativeLibrary.Load(library);
        Library = new Sdl3Library(
            methodName => NativeLibrary.GetExport(
                _handle, Sdl3Library.GetFunctionName(methodName)));

        var result = Library.Init(flags);
        if (result != 0)
            throw new SdlException("Unable to initialize SDL: " + Library.GetError().ToString());
    }

    public void Dispose()
    {
        Library.Quit();
        NativeLibrary.Free(_handle);
    }

    internal static string GetSdlPath(string? folder)
    {
        var envVar = Environment.GetEnvironmentVariable("JAWBONE_PATH_SDL3");
        if (!string.IsNullOrWhiteSpace(envVar))
            return envVar;

        string path;
        if (OperatingSystem.IsWindows())
            path = "SDL3.dll";
        else if (OperatingSystem.IsLinux())
            path = "libSDL3.so";
        else if (OperatingSystem.IsMacOS())
            path = "libSDL3.dylib";
        else
            throw new PlatformNotSupportedException();

        if (!string.IsNullOrWhiteSpace(folder))
            path = Path.Combine(folder, path);

        return path;
    }
}
