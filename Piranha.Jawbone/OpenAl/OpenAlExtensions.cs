using Microsoft.Extensions.DependencyInjection;
using Piranha.Tools;

namespace Piranha.OpenAl
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