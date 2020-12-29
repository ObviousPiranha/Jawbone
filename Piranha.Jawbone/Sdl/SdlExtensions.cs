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
            return services.AddSdl2(SdlInit.Video | SdlInit.Timer | SdlInit.Events);
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

            return services.AddNativeLibrary<ISdl2>(
                serviceProvider =>
                {
                    var bcm = serviceProvider.GetService<IBcm>();
                    bcm?.HostInit();

                    var library = SdlLoader.LoadSdl();

                    try
                    {
                        int result = library.Library.Init(flags);

                        if (result != 0)
                            throw new SdlException("Failed to initialize SDL: " + library.Library.GetError());

                        return library;
                    }
                    catch
                    {
                        library.DisposeHandle();
                        throw;
                    }
                });
        }

        public static IServiceCollection AddWindowManager(this IServiceCollection services)
        {
            return services.AddSingleton<IWindowManager, WindowManager>();
        }
    }
}
