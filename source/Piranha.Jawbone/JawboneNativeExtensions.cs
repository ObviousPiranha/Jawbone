using Microsoft.Extensions.DependencyInjection;
using System;

namespace Piranha.Jawbone;

public static class JawboneNativeExtensions
{
    public static IServiceCollection AddJawboneNativeLibraries(
        this IServiceCollection services)
    {
        var path = "PiranhaNative.dll";
        if (OperatingSystem.IsLinux())
            path = "./" + path;

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
