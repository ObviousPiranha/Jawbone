using System;

namespace Jawbone.Sdl3;

public static class SdlPropRendererCreate
{
    public static class Boolean
    {
        public static ReadOnlySpan<byte> Spirv => "SDL.renderer.create.gpu.shaders_spirv\0"u8;
        public static ReadOnlySpan<byte> Dxil => "SDL.renderer.create.gpu.shaders_dxil\0"u8;
        public static ReadOnlySpan<byte> Msl => "SDL.renderer.create.gpu.shaders_msl\0"u8;
        public static ReadOnlySpan<byte> VulkanInstance => "SDL.renderer.create.vulkan.instance\0"u8;
        public static ReadOnlySpan<byte> VulkanPhysicalDevice => "SDL.renderer.create.vulkan.physical_device\0"u8;
        public static ReadOnlySpan<byte> VulkanDevice => "SDL.renderer.create.vulkan.device\0"u8;
    }

    public static class String
    {
        public static ReadOnlySpan<byte> Name => "SDL.renderer.create.name\0"u8;
    }

    public static class Pointer
    {
        public static ReadOnlySpan<byte> Window => "SDL.renderer.create.window\0"u8;
        public static ReadOnlySpan<byte> Surface => "SDL.renderer.create.surface\0"u8;
        public static ReadOnlySpan<byte> GpuDevice => "SDL.renderer.create.gpu.device\0"u8;
    }

    public static class Number
    {
        public static ReadOnlySpan<byte> OutputColorspace => "SDL.renderer.create.output_colorspace\0"u8;
        public static ReadOnlySpan<byte> PresentVsync => "SDL.renderer.create.present_vsync\0"u8;
        public static ReadOnlySpan<byte> VulkanSurface => "SDL.renderer.create.vulkan.surface\0"u8;
        public static ReadOnlySpan<byte> VulkanGraphicsQueueFamilyIndex => "SDL.renderer.create.vulkan.graphics_queue_family_index\0"u8;
        public static ReadOnlySpan<byte> VulkanPresentQueueFamilyIndex => "SDL.renderer.create.vulkan.present_queue_family_index\0"u8;
    }
}