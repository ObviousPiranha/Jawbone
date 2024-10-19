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
        var name = C.GetLibraryName();
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
}
