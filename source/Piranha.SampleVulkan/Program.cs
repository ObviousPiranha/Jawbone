using Evergine.Bindings.Vulkan;
using Piranha.Jawbone.Extensions;
using Piranha.Jawbone.Sdl2;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Piranha.SampleVulkan;

internal class Program
{
    private static unsafe void Main(string[] args)
    {
        using var sdlProvider = new Sdl2Provider(GetLib(), SdlInit.Everything);
        var sdl = sdlProvider.Library;
        var window = sdl.CreateWindow(
            "Hello Vulkan",
            SdlWindowPos.Centered,
            SdlWindowPos.Centered,
            640,
            480,
            SdlWindow.Shown | SdlWindow.Resizable | SdlWindow.Vulkan);

        if (window.IsInvalid())
            throw new SdlException(sdl.GetError().ToString() ?? "[empty error]");

        string[] extensions = [];

        if (OperatingSystem.IsLinux())
        {
            extensions = ["VK_KHR_wayland_surface"];
            System.Runtime.InteropServices.NativeLibrary.SetDllImportResolver(
                typeof(VkApplicationInfo).Assembly,
                (lib, assembly, searchPath) =>
                {
                    if (lib == "libdl")
                        return System.Runtime.InteropServices.NativeLibrary.Load("/usr/lib/x86_64-linux-gnu/libdl.so.2");

                    return default;
                });
        }
        else if (OperatingSystem.IsWindows())
        {
            extensions = ["VK_KHR_surface", "VK_KHR_win32_surface", "VK_EXT_debug_utils"];
        }

        nint* extensionsNative = stackalloc nint[extensions.Length];
        for (int i = 0; i < extensions.Length; ++i)
            extensionsNative[i] = Marshal.StringToHGlobalAnsi(extensions[i]);

        var vulkanAppName = "Hello Vulkan\0"u8;
        var vulkanEngineName = "Custom Engine\0"u8;
        VkApplicationInfo appInfo;
        fixed (byte* a = vulkanAppName, b = vulkanEngineName)
        {
            appInfo = new VkApplicationInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO,
                pApplicationName = a,
                applicationVersion = Version(1, 0, 0),
                pEngineName = b,
                engineVersion = Version(1, 0, 0),
                apiVersion = Version(1, 0, 0),
            };
        }

        var createInfo = default(VkInstanceCreateInfo);
        createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
        createInfo.pApplicationInfo = &appInfo;
        createInfo.enabledExtensionCount = (uint)extensions.Length;
        createInfo.ppEnabledExtensionNames = (byte**)extensionsNative;
        VkInstance instance;
        var vkResult = VulkanNative.vkCreateInstance(&createInfo, null, &instance);
        if (vkResult != 0)
        {
            Console.WriteLine("Error creating vulkan instance.");
            return;
        }

        Console.WriteLine("Created vulkan instance!");


        var result = sdl.VulkanCreateSurface(window, instance.Handle, out var vulkanSurface);

        if (result == 0)
            throw new SdlException(sdl.GetError().ToString() ?? "[empty error]");
        Console.WriteLine("Created SDL window!");

        // Physical device
        // https://vkguide.dev/docs/new_chapter_1/vulkan_init_flow/
        // https://vkguide.dev/docs/new_chapter_1/vulkan_init_code/
        // https://vulkan-tutorial.com/Drawing_a_triangle/Setup/Physical_devices_and_queue_families
        {
            // https://harrylovescode.gitbooks.io/vulkan-api/content/chap03/chap03.html
            VkPhysicalDevice physicalDevice;

            uint deviceCount = 0;
            VulkanNative.vkEnumeratePhysicalDevices(instance, &deviceCount, null);

            if (deviceCount == 0)
                throw new Exception("You have no graphics, you doofus");
            
            var physicalDevices = stackalloc VkPhysicalDevice[(int)deviceCount];
            VulkanNative.vkEnumeratePhysicalDevices(instance, &deviceCount, physicalDevices);
            var options = new JsonSerializerOptions { WriteIndented = true };

            for (int i = 0; i < deviceCount; ++i)
            {
                // vkGetPhysicalDeviceProperties
                var props = default(VkPhysicalDeviceProperties);
                VulkanNative.vkGetPhysicalDeviceProperties(physicalDevices[i], &props);

                Console.WriteLine(Marshal.PtrToStringUTF8(new nint(props.deviceName)));
                Console.WriteLine(JsonSerializer.Serialize(props, options));
            }
        }

        ApplicationManager.RunBlocking(sdl, new EventHandler());

        sdl.DestroyWindow(window);
    }

    static uint Version(uint major, uint minor, uint patch)
    {
        return (major << 22) | (minor << 12) | patch;
    }

    static string GetLib()
    {
        if (OperatingSystem.IsWindows())
            return "SDL2.dll";
        else if (OperatingSystem.IsLinux())
            return "./libSDL2-2.0.so";

        throw new PlatformNotSupportedException();
    }
}
