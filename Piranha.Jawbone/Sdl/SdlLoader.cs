using System;
using System.IO;
using System.Linq;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sdl
{
    public class SdlLoader : IPlatformLoader<string?>
    {
        private static readonly SdlLoader theInstance = new SdlLoader();

        private static string ResolveName(string methodName)
        {
            if (methodName.StartsWith("Gl"))
                return "SDL_GL_" + methodName.Substring(2);
            
            return "SDL_" + methodName;
        }

        public static NativeLibraryInterface<ISdl2> LoadSdl()
        {
            var path = theInstance.CurrentPlatform() ?? throw new NullReferenceException("Failed to load SDL path.");
            return NativeLibraryInterface.FromFile<ISdl2>(path, ResolveName);
        }

        private SdlLoader()
        {
        }

        public string? Linux()
        {
            return Platform.FindLibs("libSDL2-2.0.so*", "libSDL2.so*");
        }

        public string? macOS() => "/usr/local/opt/sdl2/lib/libSDL2.dylib";

        public string? Windows() => "SDL2.dll";
    }
}
