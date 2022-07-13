using System;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Opus;

public static class OpusExtensions
{
    public static IServiceCollection AddOpus(this IServiceCollection services)
    {
        return services.AddNativeLibrary(
            _ => NativeLibraryInterface.FromFile<IOpus>(
                "/usr/lib/x86_64-linux-gnu/libopus.so.0",
                methodName => NativeLibraryInterface.PascalCaseToSnakeCase("opus", methodName)));
    }
}