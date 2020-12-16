using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sdl
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
