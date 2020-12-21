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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && File.Exists(BcmLibrary))
            {
                services.AddNativeLibrary<IBcm>(
                    _ => NativeLibraryInterface.FromFile<IBcm>(
                        BcmLibrary, _ => "bcm_host_init"));
            }

            return services.AddNativeLibrary<ISdl2>(
                _ => SdlLoader.LoadSdl());
        }
    }
}
