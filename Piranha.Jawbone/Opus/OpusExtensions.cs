using Microsoft.Extensions.DependencyInjection;

namespace Piranha.Jawbone.Opus;

public static class OpusExtensions
{
    public static IServiceCollection AddOpus(this IServiceCollection services)
    {
        return services.AddSingleton<OpusProvider>(_ => OpusProvider.Create());
    }

    public static IServiceCollection AddOpus(this IServiceCollection services, string libraryPath)
    {
        return services.AddSingleton<OpusProvider>(_ => OpusProvider.Create(libraryPath));
    }
}