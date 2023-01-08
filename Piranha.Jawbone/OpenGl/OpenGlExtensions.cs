using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.OpenGl;

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
