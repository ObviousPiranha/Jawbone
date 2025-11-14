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

    public static unsafe void PushVertexUniformMatrix(
        nint commandBuffer,
        uint slotIndex,
        in Matrix4x4 matrix)
    {
        fixed (void* m = &matrix)
        {
            Sdl.PushGpuVertexUniformData(
                commandBuffer,
                slotIndex,
                new(m),
                (uint)sizeof(Matrix4x4));
        }
    }

    public static unsafe void PushFragmentUniformMatrix(
        nint commandBuffer,
        uint slotIndex,
        in Matrix4x4 matrix)
    {
        fixed (void* m = &matrix)
        {
            Sdl.PushGpuFragmentUniformData(
                commandBuffer,
                slotIndex,
                new(m),
                (uint)sizeof(Matrix4x4));
        }
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
        ];

        var result = pairs.ToFrozenDictionary();
        return result;
    }
}
