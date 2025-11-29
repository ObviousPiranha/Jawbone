using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Jawbone.Sdl3;

public static class SdlExtensions
{
    private static readonly FrozenDictionary<Type, SdlGpuVertexElementFormat> TypeToAttribute = CreateTypeMapping();

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

    public static (int major, int minor, int micro) GetVersion()
    {
        var version = Sdl.GetVersion();
        return (
            version / 1000000,
            version / 1000 % 1000,
            version % 1000);
    }

    public static IEnumerable<string> EnumerateGpuDrivers()
    {
        var gpuDriverCount = Sdl.GetNumGpuDrivers();
        for (int i = 0; i < gpuDriverCount; ++i)
        {
            var gpuDriver = Sdl.GetGpuDriver(i).ToString();
            if (!string.IsNullOrWhiteSpace(gpuDriver))
                yield return gpuDriver;
        }
    }

    public static unsafe void PushVertexUniform<T>(
        nint commandBuffer,
        uint slotIndex,
        in T value) where T : unmanaged
    {
        fixed (void* p = &value)
        {
            Sdl.PushGpuVertexUniformData(
                commandBuffer,
                slotIndex,
                new(p),
                (uint)sizeof(T));
        }
    }

    public static unsafe void PushFragmentUniform<T>(
        nint commandBuffer,
        uint slotIndex,
        in T value) where T : unmanaged
    {
        fixed (void* p = &value)
        {
            Sdl.PushGpuFragmentUniformData(
                commandBuffer,
                slotIndex,
                new(p),
                (uint)sizeof(T));
        }
    }

    public static void BindGpuVertexBuffers(
        nint renderPass,
        uint firstSlot,
        params ReadOnlySpan<SdlGpuBufferBinding> bindings)
    {
        Sdl.BindGpuVertexBuffers(
            renderPass, firstSlot, bindings[0], (uint)bindings.Length);
    }

    public static void BindGpuFragmentSamplers(
        nint renderPass,
        uint firstSlot,
        params ReadOnlySpan<SdlGpuTextureSamplerBinding> bindings)
    {
        Sdl.BindGpuFragmentSamplers(
            renderPass, firstSlot, bindings[0], (uint)bindings.Length);
    }

    public static SdlGpuVertexAttribute[] GetGpuVertexAttributes<T>(uint bufferSlot = 0) where T : unmanaged
    {
        var fields = typeof(T).GetFields();
        var result = new SdlGpuVertexAttribute[fields.Length];
        for (int i = 0; i < fields.Length; ++i)
        {
            var fieldInfo = fields[i];
            var name = fieldInfo.Name;

            result[i] = new SdlGpuVertexAttribute
            {
                Location = (uint)i,
                BufferSlot = bufferSlot,
                Format = TypeToAttribute[fieldInfo.FieldType],
                Offset = (uint)Marshal.OffsetOf<T>(fieldInfo.Name)
            };
        }
        return result;
    }

    private static FrozenDictionary<Type, SdlGpuVertexElementFormat> CreateTypeMapping()
    {
        IEnumerable<KeyValuePair<Type, SdlGpuVertexElementFormat>> pairs =
        [
            new(typeof(float), SdlGpuVertexElementFormat.Float),
            new(typeof(Vector2), SdlGpuVertexElementFormat.Float2),
            new(typeof(Vector3), SdlGpuVertexElementFormat.Float3),
            new(typeof(Vector4), SdlGpuVertexElementFormat.Float4),
            new(typeof(ColorRgba32), SdlGpuVertexElementFormat.Ubyte4Norm)
        ];

        var result = pairs.ToFrozenDictionary();
        return result;
    }
}
