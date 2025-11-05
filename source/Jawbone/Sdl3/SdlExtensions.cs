using Microsoft.Extensions.DependencyInjection;

namespace Jawbone.Sdl3;

public static class SdlExtensions
{
    public static IServiceCollection AddAudioManager(this IServiceCollection services)
    {
        return services.AddSingleton<IAudioManager, AudioManager>();
    }

    public static CBool ThrowOnSdlFailure(this CBool result, string? message)
    {
        if (!result)
            SdlException.Throw(message);
        return result;
    }

    public static nint ThrowOnSdlFailure(this nint result, string? message)
    {
        if (result == default)
            SdlException.Throw(message);
        return result;
    }
}
