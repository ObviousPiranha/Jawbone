using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.OpenAl
{
    public static class OpenAlExtensions
    {
        public static IServiceCollection AddOpenAl(this IServiceCollection services)
        {
            return services.AddNativeLibrary<IOpenAl>(
                _ => OpenAlLoader.LoadOpenAl());
        }

        public static IServiceCollection AddAudioSystem(this IServiceCollection services)
        {
            return services.AddSingleton<IAudioSystem, AudioSystem>();
        }
    }
}
