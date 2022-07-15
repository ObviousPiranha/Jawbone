using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Opus;

public static class OpusExtensions
{
    private static readonly string[] LibPaths = new string[]
    {
        "/usr/lib/x86_64-linux-gnu/libopus.so.0",
        "/usr/lib/libopus.so"
    };

    public static IServiceCollection AddOpus(this IServiceCollection services)
    {
        // TODO: Make gooder lib finder.
        var path = LibPaths.First(File.Exists);
        
        return services.AddNativeLibrary(
            _ => NativeLibraryInterface.FromFile<IOpus>(
                path,
                methodName => NativeLibraryInterface.PascalCaseToSnakeCase("opus", methodName)));
    }
}