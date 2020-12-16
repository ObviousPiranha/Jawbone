using Microsoft.Extensions.DependencyInjection;
using Piranha.Tools;

namespace Piranha.Sdl
{
    public static class SdlExtensions
    {
        public static IServiceCollection AddSdl2(this IServiceCollection services)
        {
            return services.AddNativeLibrary<ISdl2>(
                _ => SdlLoader.LoadSdl());
        }
    }
}