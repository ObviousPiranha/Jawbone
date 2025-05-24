using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.OpenGl;

public readonly struct ShaderInputMapper
{
    private static KeyValuePair<Type, CommonVertexInfo> Pair(Type type, int glSize, uint glType)
        => KeyValuePair.Create(type, new CommonVertexInfo { Size = glSize, Type = glType });
    private static readonly FrozenDictionary<Type, CommonVertexInfo> CommonVertexInfoByType =
        new KeyValuePair<Type, CommonVertexInfo>[]
        {
            Pair(typeof(Half), 1, Gl.HalfFloat),
            Pair(typeof(Vector2<Half>), 2, Gl.HalfFloat),
            Pair(typeof(Vector3<Half>), 3, Gl.HalfFloat),
            Pair(typeof(Vector4<Half>), 4, Gl.HalfFloat),
            Pair(typeof(float), 1, Gl.Float),
            Pair(typeof(Vector2), 2, Gl.Float),
            Pair(typeof(Vector2<float>), 2, Gl.Float),
            Pair(typeof(Vector3), 3, Gl.Float),
            Pair(typeof(Vector3<float>), 3, Gl.Float),
            Pair(typeof(Vector4), 4, Gl.Float),
            Pair(typeof(Vector4<float>), 4, Gl.Float),
            Pair(typeof(double), 1, Gl.Double),
            Pair(typeof(Vector2<double>), 2, Gl.Double),
            Pair(typeof(Vector3<double>), 3, Gl.Double),
            Pair(typeof(Vector4<double>), 4, Gl.Double),
            Pair(typeof(bool), 1, Gl.UnsignedByte),
            Pair(typeof(Vector2<bool>), 2, Gl.UnsignedByte),
            Pair(typeof(Vector3<bool>), 3, Gl.UnsignedByte),
            Pair(typeof(Vector4<bool>), 4, Gl.UnsignedByte),
            Pair(typeof(sbyte), 1, Gl.Byte),
            Pair(typeof(Vector2<sbyte>), 2, Gl.Byte),
            Pair(typeof(Vector3<sbyte>), 3, Gl.Byte),
            Pair(typeof(Vector4<sbyte>), 4, Gl.Byte),
            Pair(typeof(byte), 1, Gl.UnsignedByte),
            Pair(typeof(Vector2<byte>), 2, Gl.UnsignedByte),
            Pair(typeof(Vector3<byte>), 3, Gl.UnsignedByte),
            Pair(typeof(Vector4<byte>), 4, Gl.UnsignedByte),
            Pair(typeof(short), 1, Gl.Short),
            Pair(typeof(Vector2<short>), 2, Gl.Short),
            Pair(typeof(Vector3<short>), 3, Gl.Short),
            Pair(typeof(Vector4<short>), 4, Gl.Short),
            Pair(typeof(ushort), 1, Gl.UnsignedShort),
            Pair(typeof(Vector2<ushort>), 2, Gl.UnsignedShort),
            Pair(typeof(Vector3<ushort>), 3, Gl.UnsignedShort),
            Pair(typeof(Vector4<ushort>), 4, Gl.UnsignedShort),
            Pair(typeof(int), 1, Gl.Int),
            Pair(typeof(Vector2<int>), 2, Gl.Int),
            Pair(typeof(Vector3<int>), 3, Gl.Int),
            Pair(typeof(Vector4<int>), 4, Gl.Int),
            Pair(typeof(uint), 1, Gl.UnsignedInt),
            Pair(typeof(Vector2<uint>), 2, Gl.UnsignedInt),
            Pair(typeof(Vector3<uint>), 3, Gl.UnsignedInt),
            Pair(typeof(Vector4<uint>), 4, Gl.UnsignedInt),
            Pair(typeof(ColorRgb24), 3, Gl.UnsignedByte),
            Pair(typeof(ColorRgba32), 4, Gl.UnsignedByte),
            Pair(typeof(PackedUnsignedVector4), (int)Gl.Bgra, Gl.UnsignedInt2101010Rev)
        }.ToFrozenDictionary();

    public static ShaderInputMapper Create<T>(
        OpenGlLibrary gl,
        uint program,
        Func<string, string>? fieldNameToShaderName = default
        ) where T : unmanaged
    {
        var builder = ImmutableArray.CreateBuilder<VertexInfo>();
        foreach (var fieldInfo in typeof(T).GetFields())
        {
            var name = fieldInfo.Name;
            var normalized = false;
            var attribute = fieldInfo.GetCustomAttribute<ShaderInputAttribute>(false);

            if (attribute is not null)
            {
                name = attribute.Name;
                normalized = (attribute.Settings & ShaderInputSettings.Normalized) == ShaderInputSettings.Normalized;
            }
            else if (fieldNameToShaderName is not null)
            {
                name = fieldNameToShaderName.Invoke(fieldInfo.Name);
            }
            else
            {
                continue;
            }

            var info = new VertexInfo
            {
                Common = CommonVertexInfoByType[fieldInfo.FieldType],
                Index = gl.GetAttribLocation(program, name),
                Normalized = normalized ? Gl.True : Gl.False,
                Offset = Marshal.OffsetOf<T>(fieldInfo.Name).ToInt32()
            };

            if (info.Index == -1)
                throw new OpenGlException($"Unable to locate attribute '{name}' for shader program ({program}).");

            builder.Add(info);
        }

        return new(builder.ToImmutable(), Unsafe.SizeOf<T>());
    }

    private readonly ImmutableArray<VertexInfo> _vertexInfo;
    private readonly int _stride;

    private ShaderInputMapper(ImmutableArray<VertexInfo> vertexInfo, int stride)
    {
        _vertexInfo = vertexInfo;
        _stride = stride;
    }

    public void Enable(OpenGlLibrary gl)
    {
        foreach (var vertexInfo in _vertexInfo)
            gl.EnableVertexAttribArray(vertexInfo.Index);
    }

    public void VertexAttribPointers(OpenGlLibrary gl)
    {
        foreach (var vertexInfo in _vertexInfo)
        {
            gl.VertexAttribPointer(
                vertexInfo.Index,
                vertexInfo.Common.Size,
                vertexInfo.Common.Type,
                vertexInfo.Normalized,
                _stride,
                vertexInfo.Offset);
        }
    }

    public void Disable(OpenGlLibrary gl)
    {
        var last = _vertexInfo.Length - 1;
        for (int i = 0; i < _vertexInfo.Length; ++i)
            gl.DisableVertexAttribArray(_vertexInfo[last - i].Index);
    }
}
