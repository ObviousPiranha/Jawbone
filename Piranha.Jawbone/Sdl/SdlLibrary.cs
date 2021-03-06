using System;
using System.IO;
using System.Linq;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sdl
{
    public sealed class SdlLibrary : IPlatformLoader<string?>, IDisposable
    {
        private static string ResolveName(string methodName)
        {
            if (methodName.StartsWith("Gl"))
                return "SDL_GL_" + methodName.Substring(2);
            
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

        public string? macOS() => "/usr/local/opt/sdl2/lib/libSDL2.dylib";

        public string? Windows() => "SDL2.dll";

        public void Dispose()
        {
            _nativeLibraryInterface.Library.Quit();
            _nativeLibraryInterface.Dispose();
        }
    }
}
