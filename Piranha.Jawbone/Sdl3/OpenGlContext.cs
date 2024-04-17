using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Extensions;
using Piranha.Jawbone.OpenGl;
using System;
using System.Linq;

namespace Piranha.Jawbone.Sdl3;

public readonly struct OpenGlContext
{
    public readonly nint SdlGlContext { get; init; }
    public readonly OpenGlLibrary OpenGl { get; init; }

    public static OpenGlContext Create(
        Sdl3Library sdl,
        nint sdlWindow,
        ILogger? logger = null)
    {
        sdl.GlSetAttribute(SdlGl.RedSize, 8);
        sdl.GlSetAttribute(SdlGl.GreenSize, 8);
        sdl.GlSetAttribute(SdlGl.BlueSize, 8);
        sdl.GlSetAttribute(SdlGl.AlphaSize, 8);
        // _sdl.GlSetAttribute(SdlGl.DepthSize, 24);
        sdl.GlSetAttribute(SdlGl.DoubleBuffer, 1);

        if (Platform.IsRaspberryPi)
        {
            logger?.LogDebug("configuring OpenGL ES 3.0");
            sdl.GlSetAttribute(SdlGl.ContextMajorVersion, 3);
            sdl.GlSetAttribute(SdlGl.ContextMinorVersion, 0);
            sdl.GlSetAttribute(SdlGl.ContextProfileMask, SdlGlContextProfile.Es);
        }
        else if (OperatingSystem.IsMacOS())
        {
            logger?.LogDebug("configuring OpenGL 3.2");
            sdl.GlSetAttribute(SdlGl.ContextMajorVersion, 3);
            sdl.GlSetAttribute(SdlGl.ContextMinorVersion, 2);
            sdl.GlSetAttribute(SdlGl.ContextProfileMask, SdlGlContextProfile.Core);
        }

        var contextPtr = sdl.GlCreateContext(sdlWindow);

        if (contextPtr.IsInvalid())
        {
            throw new SdlException(
                "Unable to create GL context: " + sdl.GetError());
        }

        try
        {
            if (sdl.GlLoadLibrary() != 0)
            {
                throw new SdlException(
                    "Unable to load GL library: " + sdl.GetError());
            }
            var gl = new OpenGlLibrary(
                methodName => sdl.GlGetProcAddress("gl" + methodName));

            gl.GetIntegerv(Gl.MaxTextureSize, out var maxTextureSize);

            sdl.GetVersion(out var version);

            if (logger is not null)
            {
                var log = string.Concat(
                    "SDL version: ",
                    version.ToString(),
                    Environment.NewLine,
                    "SDL video driver: ",
                    sdl.GetCurrentVideoDriver(),
                    Environment.NewLine,
                    "OpenGL version: ",
                    gl.GetString(Gl.Version),
                    Environment.NewLine,
                    "OpenGL shading language version: ",
                    gl.GetString(Gl.ShadingLanguageVersion),
                    Environment.NewLine,
                    "OpenGL vendor: ",
                    gl.GetString(Gl.Vendor),
                    Environment.NewLine,
                    "OpenGL renderer: ",
                    gl.GetString(Gl.Renderer),
                    Environment.NewLine,
                    "OpenGL max texture size: ",
                    maxTextureSize);

                logger.LogInformation("{versionInfo}", log);

                var driverCount = sdl.GetNumVideoDrivers();
                var drivers = Enumerable.Range(0, driverCount).Select(n => sdl.GetVideoDriver(n));
                logger.LogDebug("Drivers: {drivers}", string.Join(", ", drivers));
            }

            return new OpenGlContext { SdlGlContext = contextPtr, OpenGl = gl };
        }
        catch
        {
            sdl.GlDeleteContext(contextPtr);
            contextPtr = default;
            throw;
        }
    }
}
