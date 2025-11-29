using System;

namespace Jawbone.Sdl3;

public static class SdlPropGpuDeviceCreate
{
    public static class Boolean
    {
        public static ReadOnlySpan<byte> DebugMode => "SDL.gpu.device.create.debugmode\0"u8;
        public static ReadOnlySpan<byte> PreferLowPower => "SDL.gpu.device.create.preferlowpower\0"u8;
        public static ReadOnlySpan<byte> Verbose => "SDL.gpu.device.create.verbose\0"u8;
        public static ReadOnlySpan<byte> FeatureClipDistance => "SDL.gpu.device.create.feature.clip_distance\0"u8;
        public static ReadOnlySpan<byte> FeatureDepthClamping => "SDL.gpu.device.create.feature.depth_clamping\0"u8;
        public static ReadOnlySpan<byte> FeatureIndirectDrawFirstInstance => "SDL.gpu.device.create.feature.indirect_draw_first_instance\0"u8;
        public static ReadOnlySpan<byte> FeatureAnisotropy => "SDL.gpu.device.create.feature.anisotropy\0"u8;
        public static ReadOnlySpan<byte> ShadersPrivate => "SDL.gpu.device.create.shaders.private\0"u8;
        public static ReadOnlySpan<byte> ShadersSpirv => "SDL.gpu.device.create.shaders.spirv\0"u8;
        public static ReadOnlySpan<byte> ShadersDxbc => "SDL.gpu.device.create.shaders.dxbc\0"u8;
        public static ReadOnlySpan<byte> ShadersDxil => "SDL.gpu.device.create.shaders.dxil\0"u8;
        public static ReadOnlySpan<byte> ShadersMsl => "SDL.gpu.device.create.shaders.msl\0"u8;
        public static ReadOnlySpan<byte> ShadersMetalLib => "SDL.gpu.device.create.shaders.metallib\0"u8;
        public static ReadOnlySpan<byte> D3d12AllowFewerResourceSlots => "SDL.gpu.device.create.d3d12.allowtier1resourcebinding\0"u8;
    }

    public static class String
    {
        public static ReadOnlySpan<byte> Name => "SDL.gpu.device.create.name\0"u8;
        public static ReadOnlySpan<byte> D3d12SemanticName => "SDL.gpu.device.create.d3d12.semantic\0"u8;
    }
}
