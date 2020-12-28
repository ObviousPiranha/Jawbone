using System;
using System.Text;

namespace Piranha.Jawbone.OpenGl
{
    public static class GlTools
    {
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
    }
}
