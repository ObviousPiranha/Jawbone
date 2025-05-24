using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Jawbone.Sdl2;

sealed class Sdl2Provider : IDisposable
{
    private readonly nint _handle;

    public Sdl2Library Library { get; }

    public Sdl2Provider(string library, SdlInit flags)
    {
        _handle = NativeLibrary.Load(library);
        Library = new Sdl2Library(
            methodName => NativeLibrary.GetExport(
                _handle, Sdl2Library.GetFunctionName(methodName)));

        if (OperatingSystem.IsLinux())
            Library.SetHint("SDL_VIDEODRIVER", "wayland,x11");
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
        var envVar = Environment.GetEnvironmentVariable("JAWBONE_PATH_SDL2");
        if (!string.IsNullOrWhiteSpace(envVar))
            return envVar;

        string path;
        if (OperatingSystem.IsWindows())
            path = "SDL2.dll";
        else if (OperatingSystem.IsLinux())
            path = "libSDL2-2.0.so";
        else if (OperatingSystem.IsMacOS())
            path = "libSDL2-2.0.dylib";
        else
            throw new PlatformNotSupportedException();

        if (!string.IsNullOrWhiteSpace(folder))
            path = Path.Combine(folder, path);

        return path;
    }
}
