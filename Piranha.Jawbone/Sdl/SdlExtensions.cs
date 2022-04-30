using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Bcm;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sdl
{
    public static class SdlExtensions
    {
        private const string BcmLibrary = "/opt/vc/lib/libbcm_host.so";

        public static IServiceCollection AddSdl2(this IServiceCollection services)
        {
            return services.AddSdl2(SdlInit.Everything);
        }

        public static IServiceCollection AddSdl2(
            this IServiceCollection services,
            uint flags)
        {
            var isVideoEnabled = (flags & SdlInit.Video) == SdlInit.Video;
            if (isVideoEnabled && RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && File.Exists(BcmLibrary))
            {
                services.AddNativeLibrary<IBcm>(
                    _ => NativeLibraryInterface.FromFile<IBcm>(
                        BcmLibrary, name => name));
            }

            return services
                .AddSingleton<SdlLibrary>(
                    serviceProvider =>
                    {
                        var bcm = serviceProvider.GetService<IBcm>();
                        bcm?.HostInit();

                        var library = new SdlLibrary(flags);
                        return library;
                    })
                .AddSingleton<ISdl2>(
                    serviceProvider => serviceProvider.GetRequiredService<SdlLibrary>().Library);
        }

        public static IServiceCollection AddWindowManager(this IServiceCollection services)
        {
            return services.AddSingleton<IWindowManager, WindowManager>();
        }

        public static IServiceCollection AddAudioManager(this IServiceCollection services)
        {
            return services.AddSingleton<IAudioManager, AudioManager>();
        }
    }
}
