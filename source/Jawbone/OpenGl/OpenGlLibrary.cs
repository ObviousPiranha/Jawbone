using Jawbone.Generation;
using System.Numerics;

namespace Jawbone.OpenGl;

[MapNativeFunctions]
public sealed partial class OpenGlLibrary
{
    public partial void ActiveTexture(uint texture);
    public partial void AttachShader(uint program, uint shader);
    public partial void BindBuffer(uint target, uint buffer);
    public partial void BindTexture(uint target, uint texture);
    public partial void BindVertexArray(uint array);
    public partial void BlendFunc(uint sfactor, uint dfactor);
    public partial void BufferData(uint target, nint size, in byte data, uint useage);
    public partial void BufferData(uint target, nint size, nint data, uint useage);
    public partial void BufferSubData(uint target, nint offset, nint size, in byte data);
    public partial void Clear(uint mask);
    public partial void ClearColor(float red, float green, float blue, float alpha);
    public partial void CompileShader(uint shader);
    public partial uint CreateProgram();
    public partial uint CreateShader(uint shaderType);
    public partial void DeleteBuffers(int n, in uint buffers);
    public partial void DeleteProgram(uint program);
    public partial void DeleteShader(uint shader);
    public partial void DeleteTextures(int n, in uint textures);
    public partial void DeleteVertexArrays(int n, in uint arrays);
    public partial void Disable(uint cap);
    public partial void DisableVertexAttribArray(int index);
    public partial void DrawArrays(uint mode, int first, int count);
    public partial void DrawArraysInstanced(uint mode, int first, int count, int instanceCount);
    public partial void Enable(uint cap);
    public partial void EnableVertexAttribArray(int index);
    public partial void Flush();
    public partial int GetAttribLocation(uint program, string name);
    public partial void GetBufferParameteriv(uint target, uint value, out int data);
    public partial void GenBuffers(int n, out uint buffers);
    public partial void GenTextures(int n, out uint textures);
    public partial void GenVertexArrays(int n, out uint arrays);
    public partial uint GetError();
    public partial void GetIntegerv(uint pname, out int value);
    public partial void GetProgramiv(uint program, uint pname, out int parameters);
    public partial void GetProgramInfoLog(uint program, int maxLength, out int length, out byte infoLog);
    public partial void GetShaderInfoLog(uint shader, int maxLength, out int length, out byte infoLog);
    public partial void GetShaderiv(uint shader, uint pname, out uint parameters);
    public partial CString GetString(uint name);
    public partial int GetUniformLocation(uint program, string name);
    public partial void LinkProgram(uint program);
    public partial void ShaderSource(uint shader, int count, in nint strings, in int lengths);
    public partial void TexImage2D(
        uint target,
        int level,
        uint internalFormat,
        int width,
        int height,
        int border,
        uint format,
        uint type,
        in byte data);
    public partial void TexImage2D(
        uint target,
        int level,
        uint internalFormat,
        int width,
        int height,
        int border,
        uint format,
        uint type,
        nint data);
    public partial void TexImage3D(
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

    public partial void TexImage3D(
        uint target,
        int level,
        uint internalFormat,
        int width,
        int height,
        int depth,
        int border,
        uint format,
        uint type,
        nint data);
    public partial void TexParameteri(uint target, uint name, uint param);
    public partial void TexSubImage3D(
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
        nint data);

    public partial void TexSubImage3D(
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

    public partial void Uniform1i(int location, int v0);
    public partial void Uniform1f(int location, float v0);
    public partial void UniformMatrix4fv(
        int location,
        int count,
        uint transpose,
        in Matrix4x4 matrix);
    public partial void UseProgram(uint program);
    public partial void VertexAttribPointer(
        int index,
        int size,
        uint type,
        uint normalized,
        int stride,
        nint pointer);
    public partial void Viewport(int x, int y, int width, int height);
}
