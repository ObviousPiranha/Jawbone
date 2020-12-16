using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Tools
{
    public interface IPlatformLoader<T>
    {
        T Windows();
        T macOS();
        T Linux();
    }

    public interface IPlatformAction
    {
        void Windows();
        void macOS();
        void Linux();
    }

    public readonly struct PlatformData<T>
    {
        public T CurrentPlatform
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return Windows;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return macOS;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return Linux;
                else
                    throw new NotSupportedException("Unknown platform.");
            }
        }

        public T Windows { get; }
        public T macOS { get; }
        public T Linux { get; }

        public PlatformData(T allPlatforms)
        {
            Windows = allPlatforms;
            macOS = allPlatforms;
            Linux = allPlatforms;
        }

        public PlatformData(
            T windows,
            T unix)
        {
            Windows = windows;
            macOS = unix;
            Linux = unix;
        }

        public PlatformData(
            T windows,
            T mac,
            T linux)
        {
            Windows = windows;
            macOS = mac;
            Linux = linux;
        }

        public override string? ToString() => CurrentPlatform?.ToString();
    }

    public static class Platform
    {
        public const string PiLibFolder = "/usr/lib/arm-linux-gnueabihf";

        public static readonly bool IsRaspberryPi = System.IO.Directory.Exists(PiLibFolder);
        public static readonly string ShaderPath = IsRaspberryPi ? "es300" : "gl150";

        private static readonly string[] LibFolders = new string[]
        {
            "/usr/lib/x86_64-linux-gnu",
            PiLibFolder,
            "/usr/lib"
        };
        
        
        // https://wiki.libsdl.org/SDL_CreateRGBSurfaceFrom
        public static readonly uint Rmask = IsRaspberryPi ? (uint)0x00ff0000 : (uint)0x000000ff;
        public static readonly uint Gmask = IsRaspberryPi ? (uint)0x0000ff00 : (uint)0x0000ff00;
        public static readonly uint Bmask = IsRaspberryPi ? (uint)0x000000ff : (uint)0x00ff0000;
        public static readonly uint Amask = IsRaspberryPi ? (uint)0x00000000 : (uint)0xff000000;

        public static string GetShaderPath(string shaderId) => ShaderPath + "." + shaderId;

        public static string? FindLib(string libPattern)
        {
            foreach (var libFolder in LibFolders)
            {
                if (!Directory.Exists(libFolder))
                    continue;

                var result = Directory
                    .EnumerateFiles(libFolder, libPattern)
                    .FirstOrDefault();

                if (result != null)
                    return result;
            }

            return null;
        }

        public static string? FindLibs(params string[] libPatterns)
        {
            foreach (var libPattern in libPatterns)
            {
                var result = FindLib(libPattern);

                if (result != null)
                    return result;
            }

            return null;
        }

        public static void Invoke(Action windows, Action mac, Action linux)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                windows();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                mac();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                linux();
            else
                throw new NotSupportedException("Unknown platform.");
        }
        
        public static T CurrentPlatform<T>(this IPlatformLoader<T> platform)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return platform.Windows();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return platform.macOS();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return platform.Linux();
            else
                throw new NotSupportedException("Unknown platform.");
        }

        public static void CurrentPlatform(this IPlatformAction platform)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                platform.Windows();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                platform.macOS();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                platform.Linux();
            else
                throw new NotSupportedException("Unknown platform.");
        }
    }
}
