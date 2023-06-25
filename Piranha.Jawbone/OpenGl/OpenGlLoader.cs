using System;
using System.IO;
using System.Linq;

namespace Piranha.Jawbone.OpenGl;

public class OpenGlLoader
{
    public static NativeLibraryInterface<IOpenGl> Load()
    {
        if (OperatingSystem.IsWindows())
        {
            return WindowsOpenGlLoader.Load();
        }
        else if (OperatingSystem.IsLinux())
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
        else if (OperatingSystem.IsMacOS())
        {
            return NativeLibraryInterface.FromFile<IOpenGl>(
                "/System/Library/Frameworks/OpenGL.framework/Versions/A/Libraries/libGL.dylib",
                name => "gl" + name);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
