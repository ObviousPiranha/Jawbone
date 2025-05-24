using System;
using System.Runtime.CompilerServices;

namespace Jawbone.OpenGl;

public static class OpenGlExtensions
{
    public static void LogProgress(
        this OpenGlLibrary gl,
        [CallerFilePath] string? file = null,
        [CallerMemberName] string? caller = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        if (!gl.DumpErrors(file, caller, lineNumber))
            Console.WriteLine($"{file} - {caller} : {lineNumber} - No errors");
    }

    public static string? GetErrorName(uint err)
    {
        string? errorName = err switch
        {
            Gl.InvalidEnum => "GL_INVALID_ENUM",
            Gl.InvalidValue => "GL_INVALID_VALUE",
            Gl.InvalidOperation => "GL_INVALID_OPERATION",
            Gl.StackOverflow => "GL_STACK_OVERFLOW",
            Gl.OutOfMemory => "GL_OUT_OF_MEMORY",
            Gl.InvalidFramebufferOperation => "GL_INVALID_FRAMEBUFFER_OPERATION",
            Gl.ContextLost => "GL_CONTEXT_LOST",
            _ => null
        };

        return errorName;
    }

    public static bool DumpErrors<T>(
        this OpenGlLibrary gl,
        T arg,
        Action<T, string> action,
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
            var errorName = GetErrorName(err);
            if (errorName is not null)
            {
                var message = $"[{file}] {caller}({lineNumber}): {errorName}";
                action.Invoke(arg, message);
            }
        }

        return result;
    }

    public static bool DumpErrors(
        this OpenGlLibrary gl,
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
                case Gl.InvalidEnum:
                    Console.WriteLine("GL_INVALID_ENUM");
                    break;
                case Gl.InvalidValue:
                    Console.WriteLine("GL_INVALID_VALUE");
                    break;
                case Gl.InvalidOperation:
                    Console.WriteLine("GL_INVALID_OPERATION");
                    break;
                case Gl.StackOverflow:
                    Console.WriteLine("GL_STACK_OVERFLOW");
                    break;
                case Gl.StackUnderflow:
                    Console.WriteLine("GL_STACK_UNDERFLOW");
                    break;
                case Gl.OutOfMemory:
                    Console.WriteLine("GL_OUT_OF_MEMORY");
                    break;
                case Gl.InvalidFramebufferOperation:
                    Console.WriteLine("GL_INVALID_FRAMEBUFFER_OPERATION");
                    break;
                case Gl.ContextLost:
                    Console.WriteLine("GL_CONTEXT_LOST");
                    break;
                default:
                    Console.WriteLine("Everything is fine. Nothing is broken.");
                    break;
            }
        }

        return result;
    }

    public static void TexParams(
        this OpenGlLibrary gl,
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

    public static void DeleteBuffer(this OpenGlLibrary gl, uint buffer)
    {
        gl.DeleteBuffers(1, buffer);
    }

    public static void DeleteBuffers(this OpenGlLibrary gl, ReadOnlySpan<uint> buffers)
    {
        gl.DeleteBuffers(buffers.Length, buffers[0]);
    }

    public static void DeleteTexture(this OpenGlLibrary gl, uint texture)
    {
        gl.DeleteTextures(1, texture);
    }

    public static void DeleteTextures(this OpenGlLibrary gl, ReadOnlySpan<uint> textures)
    {
        gl.DeleteTextures(textures.Length, textures[0]);
    }

    public static void DeleteVertexArray(this OpenGlLibrary gl, uint array)
    {
        gl.DeleteVertexArrays(1, array);
    }

    public static void DeleteVertexArrays(this OpenGlLibrary gl, ReadOnlySpan<uint> arrays)
    {
        gl.DeleteVertexArrays(arrays.Length, arrays[0]);
    }

    public static uint GenBuffer(this OpenGlLibrary gl)
    {
        gl.GenBuffers(1, out var buffer);
        return buffer;
    }

    public static void GenBuffers(this OpenGlLibrary gl, Span<uint> buffers)
    {
        gl.GenBuffers(buffers.Length, out buffers[0]);
    }

    public static uint GenTexture(this OpenGlLibrary gl)
    {
        gl.GenTextures(1, out var result);
        return result;
    }

    public static void GenTextures(this OpenGlLibrary gl, Span<uint> textures)
    {
        gl.GenTextures(textures.Length, out textures[0]);
    }

    public static uint GenVertexArray(this OpenGlLibrary gl)
    {
        gl.GenVertexArrays(1, out var array);
        return array;
    }

    public static void GenVertexArrays(this OpenGlLibrary gl, Span<uint> arrays)
    {
        gl.GenVertexArrays(arrays.Length, out arrays[0]);
    }

    public static int GetInteger(this OpenGlLibrary gl, uint pname)
    {
        gl.GetIntegerv(pname, out var result);
        return result;
    }
}
