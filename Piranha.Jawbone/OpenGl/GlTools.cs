using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Piranha.Jawbone.OpenGl
{
    public static class GlTools
    {
        public static uint LoadShaderProgram(
            IOpenGl gl,
            string vertexShaderPath,
            string fragmentShaderPath)
        {
            var program = gl.CreateProgram();
            var vertexSource = File.ReadAllBytes(vertexShaderPath);
            var fragmentSource = File.ReadAllBytes(fragmentShaderPath);
            var vertexShader = GlTools.LoadShader(gl, vertexSource, Gl.VertexShader);
            var fragmentShader = GlTools.LoadShader(gl, fragmentSource, Gl.FragmentShader);
            gl.AttachShader(program, vertexShader);
            gl.AttachShader(program, fragmentShader);
            gl.LinkProgram(program);
            gl.DeleteShader(fragmentShader);
            gl.DeleteShader(vertexShader);
            gl.GetProgramiv(program, Gl.LinkStatus, out var result);
            if (result == Gl.False)
            {
                gl.GetProgramiv(program, Gl.InfoLogLength, out var logLength);
                var buffer = new byte[logLength];
                gl.GetProgramInfoLog(program, buffer.Length, out var actualLength, buffer);
                var errors = Encoding.UTF8.GetString(buffer);
                throw new OpenGlException("Error linking program: " + errors);
            }

            return program;
        }
        public static uint LoadShader(
            IOpenGl gl,
            byte[] source,
            uint shaderType)
        {
            unsafe
            {
                fixed (byte* pointer = source)
                {
                    var shader = gl.CreateShader(shaderType);
                    var ptr = new IntPtr(pointer);
                    gl.ShaderSource(shader, 1, ptr, source.Length);
                    gl.CompileShader(shader);
                    gl.GetShaderiv(shader, Gl.CompileStatus, out var result);
                    if (result == Gl.False)
                    {
                        gl.GetShaderiv(shader, Gl.InfoLogLength, out var logLength);
                        var buffer = new byte[logLength];
                        gl.GetShaderInfoLog(shader, buffer.Length, out var actualLength, buffer);
                        // We can disregard the actual length because we queried the actual length up above.
                        var errors = Encoding.UTF8.GetString(buffer);
                        throw new OpenGlException("Error compiling shader: " + errors);
                    }

                    return shader;
                }
            }
        }

        public static bool TryLogErrors(
            this IOpenGl gl,
            ILogger logger,
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
                var errorEnum = err switch
                {
                    Gl.InvalidEnum => "GL_INVALID_ENUM",
                    Gl.InvalidValue => "GL_INVALID_VALUE",
                    Gl.InvalidOperation => "GL_INVALID_OPERATION",
                    Gl.StackOverflow => "GL_STACK_OVERFLOW",
                    Gl.StackUnderflow => "GL_STACK_UNDERFLOW",
                    Gl.OutOfMemory => "GL_OUT_OF_MEMORY",
                    Gl.InvalidFramebufferOperation => "GL_INVALID_FRAMEBUFFER_OPERATION",
                    Gl.ContextLost => "GL_CONTEXT_LOST",
                    _ => $"unknown error value: " + err
                };

                logger.LogError($"{file} - {caller} : {lineNumber} - {errorEnum}");
            }

            return result;
        }
    }
}
