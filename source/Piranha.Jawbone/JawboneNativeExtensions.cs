using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Piranha.Jawbone;

public static class JawboneNativeExtensions
{
    private const string DllName = "PiranhaNative.dll";

    public static IServiceCollection AddJawboneNativeLibraries(
        this IServiceCollection services,
        string? folder = null)
    {
        var path = DllName;
        if (!string.IsNullOrWhiteSpace(folder))
            path = Path.Combine(folder, DllName);
        else if (OperatingSystem.IsLinux())
            path = "./" + DllName;

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
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().StbVorbis)
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().Piranha);
    }
}
