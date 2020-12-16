using System;
using System.IO;
using System.Linq;
using Piranha.Tools;

namespace Piranha.OpenAl
{
    public class OpenAlLoader : IPlatformLoader<string?>
    {
        private static readonly OpenAlLoader theInstance = new OpenAlLoader();

        private static string ResolveName(string methodname)
        {
            switch(methodname)
            {
                case nameof(IOpenAl.CloseDevice):
                case nameof(IOpenAl.CreateContext):
                case nameof(IOpenAl.DestroyContext):
                case nameof(IOpenAl.MakeContextCurrent):
                case nameof(IOpenAl.OpenDevice):
                    return "alc" + methodname;
                default:
                    return "al" + methodname;
            }
        }

        public static NativeLibraryInterface<IOpenAl> LoadOpenAl()
        {
            var path = theInstance.CurrentPlatform() ?? throw new NullReferenceException("Failed to load OpenAL path.");
            return NativeLibraryInterface.Create<IOpenAl>(path, ResolveName);
        }

        private OpenAlLoader()
        {
        }

        public string? Linux()
        {
            return Platform.FindLib("libopenal.so*");
        }
        public string? macOS() => "/usr/local/opt/openal-soft/lib/libopenal.dylib";
        public string? Windows() => "soft_oal.dll";
    }
}