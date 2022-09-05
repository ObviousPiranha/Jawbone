using Microsoft.Extensions.DependencyInjection;

namespace Piranha.Jawbone.Net;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSocketProvider(this IServiceCollection services)
    {
        return services
            .AddSingleton<SocketProvider>();
    }
}
