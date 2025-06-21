// Code generated at 2025-06-21T19:01:47.

namespace Jawbone.OpenGl;

public sealed unsafe class OpenGlLibrary
{
    private readonly nint _fp_ActiveTexture;
    private readonly nint _fp_AttachShader;
    private readonly nint _fp_BindBuffer;
    private readonly nint _fp_BindTexture;
    private readonly nint _fp_BindVertexArray;
    private readonly nint _fp_BlendFunc;
    private readonly nint _fp_BufferData;
    private readonly nint _fp_BufferSubData;
    private readonly nint _fp_Clear;
    private readonly nint _fp_ClearColor;
    private readonly nint _fp_CompileShader;
    private readonly nint _fp_CreateProgram;
    private readonly nint _fp_CreateShader;
    private readonly nint _fp_DeleteBuffers;
    private readonly nint _fp_DeleteProgram;
    private readonly nint _fp_DeleteShader;
    private readonly nint _fp_DeleteTextures;
    private readonly nint _fp_DeleteVertexArrays;
    private readonly nint _fp_Disable;
    private readonly nint _fp_DisableVertexAttribArray;
    private readonly nint _fp_DrawArrays;
    private readonly nint _fp_DrawArraysInstanced;
    private readonly nint _fp_Enable;
    private readonly nint _fp_EnableVertexAttribArray;
    private readonly nint _fp_Flush;
    private readonly nint _fp_GenBuffers;
    private readonly nint _fp_GenTextures;
    private readonly nint _fp_GenVertexArrays;
    private readonly nint _fp_GetAttribLocation;
    private readonly nint _fp_GetBufferParameteriv;
    private readonly nint _fp_GetError;
    private readonly nint _fp_GetIntegerv;
    private readonly nint _fp_GetProgramInfoLog;
    private readonly nint _fp_GetProgramiv;
    private readonly nint _fp_GetShaderInfoLog;
    private readonly nint _fp_GetShaderiv;
    private readonly nint _fp_GetString;
    private readonly nint _fp_GetUniformLocation;
    private readonly nint _fp_LinkProgram;
    private readonly nint _fp_ShaderSource;
    private readonly nint _fp_TexImage2D;
    private readonly nint _fp_TexImage3D;
    private readonly nint _fp_TexParameteri;
    private readonly nint _fp_TexSubImage3D;
    private readonly nint _fp_Uniform1f;
    private readonly nint _fp_Uniform1i;
    private readonly nint _fp_UniformMatrix4fv;
    private readonly nint _fp_UseProgram;
    private readonly nint _fp_VertexAttribPointer;
    private readonly nint _fp_Viewport;

    public OpenGlLibrary(
        System.Func<string, nint> loader)
    {
        _fp_ActiveTexture = loader.Invoke(nameof(ActiveTexture));
        _fp_AttachShader = loader.Invoke(nameof(AttachShader));
        _fp_BindBuffer = loader.Invoke(nameof(BindBuffer));
        _fp_BindTexture = loader.Invoke(nameof(BindTexture));
        _fp_BindVertexArray = loader.Invoke(nameof(BindVertexArray));
        _fp_BlendFunc = loader.Invoke(nameof(BlendFunc));
        _fp_BufferData = loader.Invoke(nameof(BufferData));
        _fp_BufferSubData = loader.Invoke(nameof(BufferSubData));
        _fp_Clear = loader.Invoke(nameof(Clear));
        _fp_ClearColor = loader.Invoke(nameof(ClearColor));
        _fp_CompileShader = loader.Invoke(nameof(CompileShader));
        _fp_CreateProgram = loader.Invoke(nameof(CreateProgram));
        _fp_CreateShader = loader.Invoke(nameof(CreateShader));
        _fp_DeleteBuffers = loader.Invoke(nameof(DeleteBuffers));
        _fp_DeleteProgram = loader.Invoke(nameof(DeleteProgram));
        _fp_DeleteShader = loader.Invoke(nameof(DeleteShader));
        _fp_DeleteTextures = loader.Invoke(nameof(DeleteTextures));
        _fp_DeleteVertexArrays = loader.Invoke(nameof(DeleteVertexArrays));
        _fp_Disable = loader.Invoke(nameof(Disable));
        _fp_DisableVertexAttribArray = loader.Invoke(nameof(DisableVertexAttribArray));
        _fp_DrawArrays = loader.Invoke(nameof(DrawArrays));
        _fp_DrawArraysInstanced = loader.Invoke(nameof(DrawArraysInstanced));
        _fp_Enable = loader.Invoke(nameof(Enable));
        _fp_EnableVertexAttribArray = loader.Invoke(nameof(EnableVertexAttribArray));
        _fp_Flush = loader.Invoke(nameof(Flush));
        _fp_GenBuffers = loader.Invoke(nameof(GenBuffers));
        _fp_GenTextures = loader.Invoke(nameof(GenTextures));
        _fp_GenVertexArrays = loader.Invoke(nameof(GenVertexArrays));
        _fp_GetAttribLocation = loader.Invoke(nameof(GetAttribLocation));
        _fp_GetBufferParameteriv = loader.Invoke(nameof(GetBufferParameteriv));
        _fp_GetError = loader.Invoke(nameof(GetError));
        _fp_GetIntegerv = loader.Invoke(nameof(GetIntegerv));
        _fp_GetProgramInfoLog = loader.Invoke(nameof(GetProgramInfoLog));
        _fp_GetProgramiv = loader.Invoke(nameof(GetProgramiv));
        _fp_GetShaderInfoLog = loader.Invoke(nameof(GetShaderInfoLog));
        _fp_GetShaderiv = loader.Invoke(nameof(GetShaderiv));
        _fp_GetString = loader.Invoke(nameof(GetString));
        _fp_GetUniformLocation = loader.Invoke(nameof(GetUniformLocation));
        _fp_LinkProgram = loader.Invoke(nameof(LinkProgram));
        _fp_ShaderSource = loader.Invoke(nameof(ShaderSource));
        _fp_TexImage2D = loader.Invoke(nameof(TexImage2D));
        _fp_TexImage3D = loader.Invoke(nameof(TexImage3D));
        _fp_TexParameteri = loader.Invoke(nameof(TexParameteri));
        _fp_TexSubImage3D = loader.Invoke(nameof(TexSubImage3D));
        _fp_Uniform1f = loader.Invoke(nameof(Uniform1f));
        _fp_Uniform1i = loader.Invoke(nameof(Uniform1i));
        _fp_UniformMatrix4fv = loader.Invoke(nameof(UniformMatrix4fv));
        _fp_UseProgram = loader.Invoke(nameof(UseProgram));
        _fp_VertexAttribPointer = loader.Invoke(nameof(VertexAttribPointer));
        _fp_Viewport = loader.Invoke(nameof(Viewport));
    }

    public void ActiveTexture(
        uint texture)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_ActiveTexture;
        __fp(texture);
    }

    public void AttachShader(
        uint program,
        uint shader)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, uint, void
            >)_fp_AttachShader;
        __fp(program, shader);
    }

    public void BindBuffer(
        uint target,
        uint buffer)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, uint, void
            >)_fp_BindBuffer;
        __fp(target, buffer);
    }

    public void BindTexture(
        uint target,
        uint texture)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, uint, void
            >)_fp_BindTexture;
        __fp(target, texture);
    }

    public void BindVertexArray(
        uint array)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_BindVertexArray;
        __fp(array);
    }

    public void BlendFunc(
        uint sfactor,
        uint dfactor)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, uint, void
            >)_fp_BlendFunc;
        __fp(sfactor, dfactor);
    }

    public void BufferData(
        uint target,
        nint size,
        in byte data,
        uint useage)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, nint, void*, uint, void
            >)_fp_BufferData;
        fixed (void* __p_data = &data)
        {
            __fp(target, size, __p_data, useage);
        }
    }

    public void BufferData(
        uint target,
        nint size,
        nint data,
        uint useage)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, nint, nint, uint, void
            >)_fp_BufferData;
        __fp(target, size, data, useage);
    }

    public void BufferSubData(
        uint target,
        nint offset,
        nint size,
        in byte data)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, nint, nint, void*, void
            >)_fp_BufferSubData;
        fixed (void* __p_data = &data)
        {
            __fp(target, offset, size, __p_data);
        }
    }

    public void Clear(
        uint mask)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_Clear;
        __fp(mask);
    }

    public void ClearColor(
        float red,
        float green,
        float blue,
        float alpha)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            float, float, float, float, void
            >)_fp_ClearColor;
        __fp(red, green, blue, alpha);
    }

    public void CompileShader(
        uint shader)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_CompileShader;
        __fp(shader);
    }

    public uint CreateProgram()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint
            >)_fp_CreateProgram;
        var __result = __fp();
        return __result;
    }

    public uint CreateShader(
        uint shaderType)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, uint
            >)_fp_CreateShader;
        var __result = __fp(shaderType);
        return __result;
    }

    public void DeleteBuffers(
        int n,
        in uint buffers)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, void
            >)_fp_DeleteBuffers;
        fixed (void* __p_buffers = &buffers)
        {
            __fp(n, __p_buffers);
        }
    }

    public void DeleteProgram(
        uint program)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_DeleteProgram;
        __fp(program);
    }

    public void DeleteShader(
        uint shader)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_DeleteShader;
        __fp(shader);
    }

    public void DeleteTextures(
        int n,
        in uint textures)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, void
            >)_fp_DeleteTextures;
        fixed (void* __p_textures = &textures)
        {
            __fp(n, __p_textures);
        }
    }

    public void DeleteVertexArrays(
        int n,
        in uint arrays)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, void
            >)_fp_DeleteVertexArrays;
        fixed (void* __p_arrays = &arrays)
        {
            __fp(n, __p_arrays);
        }
    }

    public void Disable(
        uint cap)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_Disable;
        __fp(cap);
    }

    public void DisableVertexAttribArray(
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void
            >)_fp_DisableVertexAttribArray;
        __fp(index);
    }

    public void DrawArrays(
        uint mode,
        int first,
        int count)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, int, void
            >)_fp_DrawArrays;
        __fp(mode, first, count);
    }

    public void DrawArraysInstanced(
        uint mode,
        int first,
        int count,
        int instanceCount)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, int, int, void
            >)_fp_DrawArraysInstanced;
        __fp(mode, first, count, instanceCount);
    }

    public void Enable(
        uint cap)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_Enable;
        __fp(cap);
    }

    public void EnableVertexAttribArray(
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void
            >)_fp_EnableVertexAttribArray;
        __fp(index);
    }

    public void Flush()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void
            >)_fp_Flush;
        __fp();
    }

    public void GenBuffers(
        int n,
        out uint buffers)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, void
            >)_fp_GenBuffers;
        fixed (void* __p_buffers = &buffers)
        {
            __fp(n, __p_buffers);
        }
    }

    public void GenTextures(
        int n,
        out uint textures)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, void
            >)_fp_GenTextures;
        fixed (void* __p_textures = &textures)
        {
            __fp(n, __p_textures);
        }
    }

    public void GenVertexArrays(
        int n,
        out uint arrays)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, void
            >)_fp_GenVertexArrays;
        fixed (void* __p_arrays = &arrays)
        {
            __fp(n, __p_arrays);
        }
    }

    public int GetAttribLocation(
        uint program,
        string name)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, string, int
            >)_fp_GetAttribLocation;
        var __result = __fp(program, name);
        return __result;
    }

    public void GetBufferParameteriv(
        uint target,
        uint value,
        out int data)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, uint, void*, void
            >)_fp_GetBufferParameteriv;
        fixed (void* __p_data = &data)
        {
            __fp(target, value, __p_data);
        }
    }

    public uint GetError()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint
            >)_fp_GetError;
        var __result = __fp();
        return __result;
    }

    public void GetIntegerv(
        uint pname,
        out int value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void*, void
            >)_fp_GetIntegerv;
        fixed (void* __p_value = &value)
        {
            __fp(pname, __p_value);
        }
    }

    public void GetProgramInfoLog(
        uint program,
        int maxLength,
        out int length,
        out byte infoLog)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, void*, void*, void
            >)_fp_GetProgramInfoLog;
        fixed (void* __p_length = &length)
        fixed (void* __p_infoLog = &infoLog)
        {
            __fp(program, maxLength, __p_length, __p_infoLog);
        }
    }

    public void GetProgramiv(
        uint program,
        uint pname,
        out int parameters)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, uint, void*, void
            >)_fp_GetProgramiv;
        fixed (void* __p_parameters = &parameters)
        {
            __fp(program, pname, __p_parameters);
        }
    }

    public void GetShaderInfoLog(
        uint shader,
        int maxLength,
        out int length,
        out byte infoLog)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, void*, void*, void
            >)_fp_GetShaderInfoLog;
        fixed (void* __p_length = &length)
        fixed (void* __p_infoLog = &infoLog)
        {
            __fp(shader, maxLength, __p_length, __p_infoLog);
        }
    }

    public void GetShaderiv(
        uint shader,
        uint pname,
        out uint parameters)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, uint, void*, void
            >)_fp_GetShaderiv;
        fixed (void* __p_parameters = &parameters)
        {
            __fp(shader, pname, __p_parameters);
        }
    }

    public CString GetString(
        uint name)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, CString
            >)_fp_GetString;
        var __result = __fp(name);
        return __result;
    }

    public int GetUniformLocation(
        uint program,
        string name)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, string, int
            >)_fp_GetUniformLocation;
        var __result = __fp(program, name);
        return __result;
    }

    public void LinkProgram(
        uint program)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_LinkProgram;
        __fp(program);
    }

    public void ShaderSource(
        uint shader,
        int count,
        in nint strings,
        in int lengths)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, void*, void*, void
            >)_fp_ShaderSource;
        fixed (void* __p_strings = &strings)
        fixed (void* __p_lengths = &lengths)
        {
            __fp(shader, count, __p_strings, __p_lengths);
        }
    }

    public void TexImage2D(
        uint target,
        int level,
        uint internalFormat,
        int width,
        int height,
        int border,
        uint format,
        uint type,
        nint data)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, uint, int, int, int, uint, uint, nint, void
            >)_fp_TexImage2D;
        __fp(target, level, internalFormat, width, height, border, format, type, data);
    }

    public void TexImage2D(
        uint target,
        int level,
        uint internalFormat,
        int width,
        int height,
        int border,
        uint format,
        uint type,
        in byte data)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, uint, int, int, int, uint, uint, void*, void
            >)_fp_TexImage2D;
        fixed (void* __p_data = &data)
        {
            __fp(target, level, internalFormat, width, height, border, format, type, __p_data);
        }
    }

    public void TexImage3D(
        uint target,
        int level,
        int internalFormat,
        int width,
        int height,
        int depth,
        int border,
        uint format,
        uint type,
        in byte data)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, int, int, int, int, int, uint, uint, void*, void
            >)_fp_TexImage3D;
        fixed (void* __p_data = &data)
        {
            __fp(target, level, internalFormat, width, height, depth, border, format, type, __p_data);
        }
    }

    public void TexImage3D(
        uint target,
        int level,
        uint internalFormat,
        int width,
        int height,
        int depth,
        int border,
        uint format,
        uint type,
        nint data)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, uint, int, int, int, int, uint, uint, nint, void
            >)_fp_TexImage3D;
        __fp(target, level, internalFormat, width, height, depth, border, format, type, data);
    }

    public void TexParameteri(
        uint target,
        uint name,
        uint param)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, uint, uint, void
            >)_fp_TexParameteri;
        __fp(target, name, param);
    }

    public void TexSubImage3D(
        uint target,
        int level,
        int xOffset,
        int yOffset,
        int zOffset,
        int width,
        int height,
        int depth,
        uint format,
        uint type,
        nint data)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, int, int, int, int, int, int, uint, uint, nint, void
            >)_fp_TexSubImage3D;
        __fp(target, level, xOffset, yOffset, zOffset, width, height, depth, format, type, data);
    }

    public void TexSubImage3D(
        uint target,
        int level,
        int xOffset,
        int yOffset,
        int zOffset,
        int width,
        int height,
        int depth,
        uint format,
        uint type,
        in byte data)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, int, int, int, int, int, int, uint, uint, void*, void
            >)_fp_TexSubImage3D;
        fixed (void* __p_data = &data)
        {
            __fp(target, level, xOffset, yOffset, zOffset, width, height, depth, format, type, __p_data);
        }
    }

    public void Uniform1f(
        int location,
        float v0)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, float, void
            >)_fp_Uniform1f;
        __fp(location, v0);
    }

    public void Uniform1i(
        int location,
        int v0)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int, void
            >)_fp_Uniform1i;
        __fp(location, v0);
    }

    public void UniformMatrix4fv(
        int location,
        int count,
        uint transpose,
        in System.Numerics.Matrix4x4 matrix)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int, uint, void*, void
            >)_fp_UniformMatrix4fv;
        fixed (void* __p_matrix = &matrix)
        {
            __fp(location, count, transpose, __p_matrix);
        }
    }

    public void UseProgram(
        uint program)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_UseProgram;
        __fp(program);
    }

    public void VertexAttribPointer(
        int index,
        int size,
        uint type,
        uint normalized,
        int stride,
        nint pointer)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int, uint, uint, int, nint, void
            >)_fp_VertexAttribPointer;
        __fp(index, size, type, normalized, stride, pointer);
    }

    public void Viewport(
        int x,
        int y,
        int width,
        int height)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int, int, int, void
            >)_fp_Viewport;
        __fp(x, y, width, height);
    }
}
