using System;
using System.IO;
using System.Linq;

namespace Piranha.Jawbone;

public static class Platform
{
    public const string PiLibFolder = "/usr/lib/aarch64-linux-gnu";

    public static readonly bool IsRaspberryPi = System.IO.Directory.Exists(PiLibFolder);
    public static readonly string ShaderPath = IsRaspberryPi ? "es300" : "gl150";

    private static readonly string[] LibFolders =
    [
        Environment.CurrentDirectory,
        "/usr/lib/x86_64-linux-gnu",
        "/usr/lib/aarch64-linux-gnu",
        "/usr/lib/arm-linux-gnueabihf",
        "/usr/lib"
    ];


    // https://wiki.libsdl.org/SDL_CreateRGBSurfaceFrom
    public static readonly uint Rmask = IsRaspberryPi ? (uint)0x00ff0000 : (uint)0x000000ff;
    public static readonly uint Gmask = IsRaspberryPi ? (uint)0x0000ff00 : (uint)0x0000ff00;
    public static readonly uint Bmask = IsRaspberryPi ? (uint)0x000000ff : (uint)0x00ff0000;
    public static readonly uint Amask = IsRaspberryPi ? (uint)0x00000000 : (uint)0xff000000;

    public static string GetShaderPath(string shaderId) => ShaderPath + "." + shaderId;

    public static string? FindLib(string libPattern)
    {
        foreach (var libFolder in LibFolders)
        {
            if (!Directory.Exists(libFolder))
                continue;

            var result = Directory
                .EnumerateFiles(libFolder, libPattern)
                .FirstOrDefault();

            if (result is not null)
                return result;
        }

        return null;
    }

    public static string? FindLibs(params string[] libPatterns)
    {
        foreach (var libPattern in libPatterns)
        {
            var result = FindLib(libPattern);

            if (result is not null)
                return result;
        }

        return null;
    }
}
