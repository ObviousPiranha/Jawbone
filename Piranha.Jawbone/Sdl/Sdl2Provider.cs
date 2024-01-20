using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

sealed class Sdl2Provider : IDisposable
{
    private static readonly string[] MacPaths =
    [
        "/opt/homebrew/lib/libSDL2.dylib",
        "/usr/local/opt/sdl2/lib/libSDL2.dylib"
    ];

    private readonly nint _handle;

    public Sdl2Library Library { get; }

    public Sdl2Provider(string library, SdlInit flags)
    {
        _handle = NativeLibrary.Load(library);
        Library = new Sdl2Library(
            methodName => NativeLibrary.GetExport(
                _handle, Sdl2Library.GetFunctionName(methodName)));
        
        var result = Library.Init(flags);
        if (result != 0)
            throw new SdlException("Unable to initialize SDL: " + Library.GetError().ToString());
    }

    public void Dispose()
    {
        Library.Quit();
        NativeLibrary.Free(_handle);
    }
}