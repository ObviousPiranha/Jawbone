using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Openal
{
    public static class OpenAlExtensions
    {
        public static IServiceCollection AddOpenAl(this IServiceCollection services)
        {
            return services.AddNativeLibrary<IOpenAl>(
                _ => OpenAlLoader.LoadOpenAl());
        }
    }
}
