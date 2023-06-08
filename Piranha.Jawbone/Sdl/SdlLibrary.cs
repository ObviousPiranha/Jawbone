using System;
using System.IO;
using System.Linq;

namespace Piranha.Jawbone.Sdl;

public sealed class SdlLibrary : IPlatformLoader<string?>, IDisposable
{
    private static readonly string[] MacPaths = new string[]
    {
        "/opt/homebrew/lib/libSDL2.dylib",
        "/usr/local/opt/sdl2/lib/libSDL2.dylib"
    };

    private static string ResolveName(string methodName)
    {
        if (methodName.StartsWith("Gl"))
            return string.Concat("SDL_GL_", methodName.AsSpan(2));

        return "SDL_" + methodName;
    }

    private readonly NativeLibraryInterface<ISdl2> _nativeLibraryInterface;

    public ISdl2 Library => _nativeLibraryInterface.Library;

    public SdlLibrary(uint flags)
    {
        var path = this.CurrentPlatform() ?? throw new NullReferenceException("Failed to load SDL path.");
        _nativeLibraryInterface = NativeLibraryInterface.FromFile<ISdl2>(path, ResolveName);

        try
        {
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

    public string? Linux()
    {
        return Platform.FindLibs("libSDL2-2.0.so*", "libSDL2.so*");
    }

    public string? macOS() => MacPaths.First(File.Exists);

    public string? Windows() => "SDL2.dll";

    public void Dispose()
    {
        _nativeLibraryInterface.Library.Quit();
        _nativeLibraryInterface.Dispose();
    }
}
