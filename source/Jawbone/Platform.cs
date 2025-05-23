using System;
using System.IO;
using System.Linq;

namespace Jawbone;

public static class Platform
{
    public const string PiLibFolder = "/usr/lib/aarch64-linux-gnu";

    public static readonly bool IsRaspberryPi = Directory.Exists(PiLibFolder);
    public static readonly string ShaderPath = IsRaspberryPi ? "es300" : "gl150";

    private static readonly string[] LibFolders =
    [
        Environment.CurrentDirectory,
        "/usr/lib/x86_64-linux-gnu",
        "/usr/lib/aarch64-linux-gnu",
        "/usr/lib/arm-linux-gnueabihf",
        "/usr/lib"
    ];

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
