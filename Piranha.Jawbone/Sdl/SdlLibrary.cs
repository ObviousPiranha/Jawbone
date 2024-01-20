using System;
using System.IO;
using System.Linq;

namespace Piranha.Jawbone.Sdl;

sealed class SdlLibrary : IDisposable
{
    private static readonly string[] MacPaths =
    [
        "/opt/homebrew/lib/libSDL2.dylib",
        "/usr/local/opt/sdl2/lib/libSDL2.dylib"
    ];

    private static string ResolveName(string methodName)
    {
        if (methodName.StartsWith("Gl"))
            return string.Concat("SDL_GL_", methodName.AsSpan(2));

        return "SDL_" + methodName;
    }

    private readonly NativeLibraryInterface<ISdl2> _nativeLibraryInterface;

    public ISdl2 Library => _nativeLibraryInterface.Library;

    public SdlLibrary(SdlInit flags)
    {
        var path = GetSdlPath();
        _nativeLibraryInterface = NativeLibraryInterface.FromFile<ISdl2>(path, ResolveName);

        try
        {
            if (OperatingSystem.IsLinux())
                Library.SetHint("SDL_VIDEODRIVER", "wayland,x11");
            int result = Library.Init(flags);

            if (result != 0)
                throw new SdlException("Failed to initialize SDL: " + Library.GetError());
        }
        catch
        {
            _nativeLibraryInterface.Dispose();
            throw;
        }
    }

    public static string GetSdlPath()
    {
        if (OperatingSystem.IsWindows())
        {
            return "SDL2.dll";
        }
        else if (OperatingSystem.IsLinux())
        {
            return Platform.FindLibs("libSDL2-2.0.so*", "libSDL2.so*") ?? throw new NullReferenceException();
        }
        else if (OperatingSystem.IsMacOS())
        {
            return MacPaths.First(File.Exists);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    public void Dispose()
    {
        _nativeLibraryInterface.Library.Quit();
        _nativeLibraryInterface.Dispose();
    }
}
