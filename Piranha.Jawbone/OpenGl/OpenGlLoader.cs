using System;
using System.IO;
using System.Linq;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.OpenGl;

public class OpenGlLoader : IPlatformLoader<NativeLibraryInterface<IOpenGl>>
{
    public static NativeLibraryInterface<IOpenGl> Load() => new OpenGlLoader().CurrentPlatform();
    
    private OpenGlLoader()
    {
    }

    public NativeLibraryInterface<IOpenGl> Linux()
    {
        var lib = "/usr/lib/libGL.so";
        if (Directory.Exists(Platform.PiLibFolder))
        {
            lib = Directory.EnumerateFiles(
                Platform.PiLibFolder,
                "libGLESv2.so*").First();
        }
        else
        {
            lib = Platform.FindLib("libGL.so*") ?? throw new NullReferenceException();
        }

        return NativeLibraryInterface.FromFile<IOpenGl>(
            lib,
            name => "gl" + name);
    }

    public NativeLibraryInterface<IOpenGl> macOS()
    {
        return NativeLibraryInterface.FromFile<IOpenGl>(
            "/System/Library/Frameworks/OpenGL.framework/Versions/A/Libraries/libGL.dylib",
            name => "gl" + name);
    }

    public NativeLibraryInterface<IOpenGl> Windows()
    {
        return WindowsOpenGlLoader.Load();
    }
}
