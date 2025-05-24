using Microsoft.Extensions.DependencyInjection;

namespace Jawbone.Sdl3;

public static class SdlExtensions
{
    public static IServiceCollection AddAudioManager(this IServiceCollection services)
    {
        return services.AddSingleton<IAudioManager, AudioManager>();
    }
}
