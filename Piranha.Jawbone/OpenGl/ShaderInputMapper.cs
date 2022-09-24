using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.OpenGl;

public readonly struct ShaderInputMapper
{
    private static readonly ImmutableDictionary<Type, CommonVertexInfo> CommonVertexInfoByType = new Dictionary<Type, CommonVertexInfo>
    {
        [typeof(Vector2)] = new CommonVertexInfo { Size = 2, Type = Gl.Float },
        [typeof(Vector3)] = new CommonVertexInfo { Size = 3, Type = Gl.Float },
        [typeof(Vector4)] = new CommonVertexInfo { Size = 4, Type = Gl.Float },
        [typeof(Vector2<sbyte>)] = new CommonVertexInfo { Size = 2, Type = Gl.Byte },
        [typeof(Vector2<byte>)] = new CommonVertexInfo { Size = 2, Type = Gl.UnsignedByte },
        [typeof(Vector2<short>)] = new CommonVertexInfo { Size = 2, Type = Gl.Short },
        [typeof(Vector2<ushort>)] = new CommonVertexInfo { Size = 2, Type = Gl.UnsignedShort },
        [typeof(Vector2<int>)] = new CommonVertexInfo { Size = 2, Type = Gl.Int },
        [typeof(Vector2<uint>)] = new CommonVertexInfo { Size = 2, Type = Gl.UnsignedInt },
        [typeof(Vector3<sbyte>)] = new CommonVertexInfo { Size = 3, Type = Gl.Byte },
        [typeof(Vector3<byte>)] = new CommonVertexInfo { Size = 3, Type = Gl.UnsignedByte },
        [typeof(Vector3<short>)] = new CommonVertexInfo { Size = 3, Type = Gl.Short },
        [typeof(Vector3<ushort>)] = new CommonVertexInfo { Size = 3, Type = Gl.UnsignedShort },
        [typeof(Vector3<int>)] = new CommonVertexInfo { Size = 3, Type = Gl.Int },
        [typeof(Vector3<uint>)] = new CommonVertexInfo { Size = 3, Type = Gl.UnsignedInt },
        [typeof(Vector4<sbyte>)] = new CommonVertexInfo { Size = 4, Type = Gl.Byte },
        [typeof(Vector4<byte>)] = new CommonVertexInfo { Size = 4, Type = Gl.UnsignedByte },
        [typeof(Vector4<short>)] = new CommonVertexInfo { Size = 4, Type = Gl.Short },
        [typeof(Vector4<ushort>)] = new CommonVertexInfo { Size = 4, Type = Gl.UnsignedShort },
        [typeof(Vector4<int>)] = new CommonVertexInfo { Size = 4, Type = Gl.Int },
        [typeof(Vector4<uint>)] = new CommonVertexInfo { Size = 4, Type = Gl.UnsignedInt },
        [typeof(sbyte)] = new CommonVertexInfo { Size = 1, Type = Gl.Byte },
        [typeof(byte)] = new CommonVertexInfo { Size = 1, Type = Gl.UnsignedByte },
        [typeof(short)] = new CommonVertexInfo { Size = 1, Type = Gl.Short },
        [typeof(ushort)] = new CommonVertexInfo { Size = 1, Type = Gl.UnsignedShort },
        [typeof(int)] = new CommonVertexInfo { Size = 1, Type = Gl.Int },
        [typeof(uint)] = new CommonVertexInfo { Size = 1, Type = Gl.UnsignedInt },
        [typeof(float)] = new CommonVertexInfo { Size = 1, Type = Gl.Float },
        [typeof(double)] = new CommonVertexInfo { Size = 1, Type = Gl.Double }
    }.ToImmutableDictionary();

    public static ShaderInputMapper Create<T>(IOpenGl gl, uint program) where T : unmanaged
    {
        var builder = ImmutableArray.CreateBuilder<VertexInfo>();
        foreach (var fieldInfo in typeof(T).GetFields())
        {
            var attribute = fieldInfo.GetCustomAttribute<ShaderInputAttribute>(false);

            if (attribute is null)
                continue;
            
            var info = new VertexInfo
            {
                Common = CommonVertexInfoByType[fieldInfo.FieldType],
                Index = gl.GetAttribLocation(program, attribute.Name),
                Normalized = Gl.False,
                Offset = Marshal.OffsetOf<T>(fieldInfo.Name).ToInt32()
            };
            
            if (info.Index == -1)
                throw new OpenGlException($"Unable to locate attribute '{attribute.Name}' for shader program ({program}).");

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

    public void Enable(IOpenGl gl)
    {
        foreach (var vertexInfo in _vertexInfo)
            gl.EnableVertexAttribArray(vertexInfo.Index);
    }

    public void VertexAttribPointers(IOpenGl gl)
    {
        foreach (var vertexInfo in _vertexInfo)
        {
            gl.VertexAttribPointer(
                vertexInfo.Index,
                vertexInfo.Common.Size,
                vertexInfo.Common.Type,
                vertexInfo.Normalized,
                _stride,
                new IntPtr(vertexInfo.Offset));
        }
    }

    public void Disable(IOpenGl gl)
    {
        var last = _vertexInfo.Length - 1;
        for (int i = 0; i < _vertexInfo.Length; ++i)
            gl.DisableVertexAttribArray(_vertexInfo[last - i].Index);
    }
}
