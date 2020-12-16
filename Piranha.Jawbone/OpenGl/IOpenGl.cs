using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Piranha.Tools;

namespace Piranha.OpenGl
{
    public delegate void GenAction(int n, out uint array);

    public class OpenGlLoader : IPlatformLoader<NativeLibraryInterface<IOpenGl>>
    {
        public static NativeLibraryInterface<IOpenGl> Load() => new OpenGlLoader().CurrentPlatform();
        
        private OpenGlLoader()
        {
        }

        public NativeLibraryInterface<IOpenGl> Linux()
        {
            var lib = "/usr/lib/libGL.so";
            if (Directory.Exists(Platform.PiLibFolder))
            {
                lib = Directory.EnumerateFiles(
                    Platform.PiLibFolder,
                    "libGLESv2.so*").First();
            }
            else
            {
                lib = Platform.FindLib("libGL.so*") ?? throw new NullReferenceException();
            }

            return NativeLibraryInterface.Create<IOpenGl>(
                lib,
                name => "gl" + name);
        }

        public NativeLibraryInterface<IOpenGl> macOS()
        {
            return NativeLibraryInterface.Create<IOpenGl>(
                "/System/Library/Frameworks/OpenGL.framework/Versions/A/Libraries/libGL.dylib",
                name => "gl" + name);
        }

        public NativeLibraryInterface<IOpenGl> Windows()
        {
            return WindowsOpenGlLoader.Load();
        }
    }

    public interface IOpenGl
    {
        void ActiveTexture(uint texture);
        void AttachShader(uint program, uint shader);
        void BindBuffer(uint target, uint buffer);
        void BindTexture(uint target, uint texture);
        void BindVertexArray(uint array);
        void BlendFunc(uint sfactor, uint dfactor);
        void BufferData(uint target, IntPtr size, in float data, uint useage);
        void BufferSubData(uint target, IntPtr offset, IntPtr size, in float data);
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
        int GetAttribLocation(uint program, string name);
        void GetBufferParameteriv(uint target, uint value, out int data);
        void GenBuffers(int n, out uint buffers);
        void GenTextures(int n, out uint textures);
        void GenVertexArrays(int n, out uint arrays);
        uint GetError();
        void GetIntegerv(uint pname, out int value);
        void GetProgramiv(uint program, uint pname, out int parameters);
        void GetProgramInfoLog(uint program, int maxLength, out int length, byte[] infoLog);
        void GetShaderInfoLog(uint shader, int maxLength, out int length, byte[] infoLog);
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
            byte[]? data);

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

    public static class GlExtensions
    {
        public static void LogProgress(
            this IOpenGl gl,
            [CallerFilePath] string? file = null,
            [CallerMemberName] string? caller = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            if (!gl.DumpErrors(file, caller, lineNumber))
                Console.WriteLine($"{file} - {caller} : {lineNumber} - No errors");
        }

        public static bool DumpErrors(
            this IOpenGl gl,
            [CallerFilePath] string? file = null,
            [CallerMemberName] string? caller = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            bool result = false;

            while (true)
            {
                var err = gl.GetError();

                if (err == Gl.NoError)
                    break;

                result = true;
                Console.Write($"{file} - {caller} : {lineNumber} - ");
                
                switch (err)
                {
                    case Gl.InvalidEnum: Console.WriteLine("GL_INVALID_ENUM"); break;
                    case Gl.InvalidValue: Console.WriteLine("GL_INVALID_VALUE"); break;
                    case Gl.InvalidOperation: Console.WriteLine("GL_INVALID_OPERATION"); break;
                    case Gl.StackOverflow: Console.WriteLine("GL_STACK_OVERFLOW"); break;
                    case Gl.StackUnderflow: Console.WriteLine("GL_STACK_UNDERFLOW"); break;
                    case Gl.OutOfMemory: Console.WriteLine("GL_OUT_OF_MEMORY"); break;
                    case Gl.InvalidFramebufferOperation: Console.WriteLine("GL_INVALID_FRAMEBUFFER_OPERATION"); break;
                    case Gl.ContextLost: Console.WriteLine("GL_CONTEXT_LOST"); break;
                    default: Console.WriteLine("Everything is fine. Nothing is broken."); break;
                }
            }

            return result;
        }

        public static void TexParams(
            this IOpenGl gl,
            uint target,
            ReadOnlySpan<uint> parameters)
        {
            for (int i = 1; i < parameters.Length; i += 2)
            {
                gl.TexParameteri(
                    target,
                    parameters[i - 1],
                    parameters[i]);
            }
        }

        public static void DeleteBuffer(this IOpenGl gl, uint buffer)
        {
            gl.DeleteBuffers(1, buffer);
        }

        public static void DeleteBuffers(this IOpenGl gl, ReadOnlySpan<uint> buffers)
        {
            gl.DeleteBuffers(buffers.Length, buffers[0]);
        }

        public static void DeleteTexture(this IOpenGl gl, uint texture)
        {
            gl.DeleteTextures(1, texture);
        }

        public static void DeleteTextures(this IOpenGl gl, ReadOnlySpan<uint> textures)
        {
            gl.DeleteTextures(textures.Length, textures[0]);
        }

        public static void DeleteVertexArray(this IOpenGl gl, uint array)
        {
            gl.DeleteVertexArrays(1, array);
        }

        public static void DeleteVertexArrays(this IOpenGl gl, ReadOnlySpan<uint> arrays)
        {
            gl.DeleteVertexArrays(arrays.Length, arrays[0]);
        }

        public static uint GenBuffer(this IOpenGl gl)
        {
            gl.GenBuffers(1, out var buffer);
            return buffer;
        }

        public static void GenBuffers(this IOpenGl gl, Span<uint> buffers)
        {
            gl.GenBuffers(buffers.Length, out buffers[0]);
        }

        public static uint GenTexture(this IOpenGl gl)
        {
            gl.GenTextures(1, out var result);
            return result;
        }

        public static void GenTextures(this IOpenGl gl, Span<uint> textures)
        {
            gl.GenTextures(textures.Length, out textures[0]);
        }

        public static uint GenVertexArray(this IOpenGl gl)
        {
            gl.GenVertexArrays(1, out var array);
            return array;
        }

        public static void GenVertexArrays(this IOpenGl gl, Span<uint> arrays)
        {
            gl.GenVertexArrays(arrays.Length, out arrays[0]);
        }

        public static int GetInteger(this IOpenGl gl, uint pname)
        {
            gl.GetIntegerv(pname, out var result);
            return result;
        }
    }
}