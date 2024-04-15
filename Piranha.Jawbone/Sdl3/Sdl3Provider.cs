using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

sealed class Sdl3Provider : IDisposable
{
    private static readonly string[] MacPaths =
    [
        // TODO
    ];

    private readonly nint _handle;

    public Sdl3Library Library { get; }

    public Sdl3Provider(string library, SdlInit flags)
    {
        _handle = NativeLibrary.Load(library);
        Library = new Sdl3Library(
            methodName => NativeLibrary.GetExport(
                _handle, Sdl3Library.GetFunctionName(methodName)));

        // if (OperatingSystem.IsLinux())
        //     Library.SetHint("SDL_VIDEODRIVER", "wayland,x11");
        var result = Library.Init(flags);
        if (result != 0)
            throw new SdlException("Unable to initialize SDL: " + Library.GetError().ToString());
    }

    public void Dispose()
    {
        Library.Quit();
        NativeLibrary.Free(_handle);
    }

    internal static string GetSdlPath()
    {
        if (OperatingSystem.IsWindows())
            return "SDL3.dll";
        if (OperatingSystem.IsLinux())
            return Platform.FindLibs("libSDL3.so*") ?? throw new NullReferenceException();
        if (OperatingSystem.IsMacOS())
            return MacPaths.First(File.Exists);

        throw new PlatformNotSupportedException();
    }
}
