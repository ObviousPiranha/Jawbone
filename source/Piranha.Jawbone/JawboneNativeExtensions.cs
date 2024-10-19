using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Piranha.Jawbone;

public static class JawboneNativeExtensions
{
    public static IServiceCollection AddJawboneNativeLibraries(
        this IServiceCollection services,
        string? folder = null)
    {
        var name = GetLibraryName();
        var path = name;
        if (!string.IsNullOrWhiteSpace(folder))
            path = Path.Combine(folder, name);
        else if (OperatingSystem.IsLinux())
            path = "./" + name;

        return services
            .AddSingleton(_ => new JawboneNative(path))
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().Sqlite3)
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().StbImage)
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().StbImageWrite)
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().StbTrueType)
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().StbVorbis);
    }

    private static string GetLibraryName()
    {
        if (OperatingSystem.IsWindows())
            return "JawboneNative.dll";
        else if (OperatingSystem.IsMacOS())
            return "libJawboneNative.dylib";
        else if (OperatingSystem.IsLinux())
            return "libJawboneNative.so";

        throw new PlatformNotSupportedException();
    }
}
