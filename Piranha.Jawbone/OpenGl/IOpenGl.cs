using System;
using System.Numerics;

namespace Piranha.Jawbone.OpenGl
{
    public interface IOpenGl
    {
        void ActiveTexture(uint texture);
        void AttachShader(uint program, uint shader);
        void BindBuffer(uint target, uint buffer);
        void BindTexture(uint target, uint texture);
        void BindVertexArray(uint array);
        void BlendFunc(uint sfactor, uint dfactor);
        void BufferData(uint target, IntPtr size, in byte data, uint useage);
        void BufferData(uint target, IntPtr size, IntPtr data, uint useage);
        void BufferSubData(uint target, IntPtr offset, IntPtr size, in byte data);
        void Clear(uint mask);
        void ClearColor(float red, float green, float blue, float alpha);
        void CompileShader(uint shader);
        uint CreateProgram();
        uint CreateShader(uint shaderType);
        void DeleteBuffers(int n, in uint buffers);
        void DeleteProgram(uint program);
        void DeleteShader(uint shader);
        void DeleteTextures(int n, in uint textures);
        void DeleteVertexArrays(int n, in uint arrays);
        void Disable(uint cap);
        void DisableVertexAttribArray(int index);
        void DrawArrays(uint mode, int first, long count);
        void Enable(uint cap);
        void EnableVertexAttribArray(int index);
        void Flush();
        int GetAttribLocation(uint program, string name);
        void GetBufferParameteriv(uint target, uint value, out int data);
        void GenBuffers(int n, out uint buffers);
        void GenTextures(int n, out uint textures);
        void GenVertexArrays(int n, out uint arrays);
        uint GetError();
        void GetIntegerv(uint pname, out int value);
        void GetProgramiv(uint program, uint pname, out int parameters);
        void GetProgramInfoLog(uint program, int maxLength, out int length, out byte infoLog);
        void GetShaderInfoLog(uint shader, int maxLength, out int length, out byte infoLog);
        void GetShaderiv(uint shader, uint pname, out uint parameters);
        string GetString(uint name);
        int GetUniformLocation(uint program, string name);
        void LinkProgram(uint program);
        void ShaderSource(uint shader, int count, in IntPtr strings, in int lengths);
        void TexImage2D(
            uint target,
            int level,
            uint internalFormat,
            int width,
            int height,
            int border,
            uint format,
            uint type,
            IntPtr data);
        void TexImage3D(
            uint target,
            int level,
            int internalFormat,
            int width,
            int height,
            int depth,
            int border,
            uint format,
            uint type,
            in byte data);

        void TexImage3D(
            uint target,
            int level,
            uint internalFormat,
            int width,
            int height,
            int depth,
            int border,
            uint format,
            uint type,
            IntPtr data);
        void TexParameteri(uint target, uint name, uint param);
        void TexSubImage3D(
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
            IntPtr data);
        
        void TexSubImage3D(
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
            in byte data);

        void Uniform1i(int location, int v0);
        void UniformMatrix4fv(int location, int count, uint transpose, in Matrix4x4 matrix);
        void UseProgram(uint program);
        void VertexAttribPointer(
            int index,
            int size,
            uint type,
            uint normalized,
            int stride,
            IntPtr pointer);
        void Viewport(int x, int y, int width, int height);
    }
}
