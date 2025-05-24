using Jawbone.Extensions;
using Jawbone.OpenGl;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Jawbone.Sdl3;

public readonly struct OpenGlContext
{
    public readonly nint SdlGlContextPtr { get; init; }
    public readonly OpenGlLibrary OpenGl { get; init; }

    public static OpenGlContext Create(
        nint sdlWindow,
        ILogger? logger = null)
    {
        Sdl.GlSetAttribute(SdlGlAttr.RedSize, 8);
        Sdl.GlSetAttribute(SdlGlAttr.GreenSize, 8);
        Sdl.GlSetAttribute(SdlGlAttr.BlueSize, 8);
        Sdl.GlSetAttribute(SdlGlAttr.AlphaSize, 8);
        // _Sdl.GLSetAttribute(SdlGlAttr.DepthSize, 24);
        Sdl.GlSetAttribute(SdlGlAttr.Doublebuffer, 1);

        if (Platform.IsRaspberryPi)
        {
            logger?.LogInformation("configuring OpenGL ES 3.0");
            Sdl.GlSetAttribute(SdlGlAttr.ContextMajorVersion, 3);
            Sdl.GlSetAttribute(SdlGlAttr.ContextMinorVersion, 0);
            Sdl.GlSetAttribute(SdlGlAttr.ContextProfileMask, (int)SdlGlProfile.Es);
        }
        else
        {
            // Needed for Mac to work.
            logger?.LogInformation("configuring OpenGL core");
            Sdl.GlSetAttribute(SdlGlAttr.ContextProfileMask, (int)SdlGlProfile.Core);
        }

        var contextPtr = Sdl.GlCreateContext(sdlWindow);

        if (contextPtr.IsInvalid())
        {
            throw new SdlException(
                "Unable to create GL context: " + Sdl.GetError());
        }

        var result = Sdl.GlGetAttribute(SdlGlAttr.ShareWithCurrentContext, out var value);
        if (result)
        {
            logger?.LogInformation("SDL_GL_SHARE_WITH_CURRENT_CONTEXT: {value}", value);
        }
        else
        {
            logger?.LogWarning("Unable to get attribute SDL_GL_SHARE_WITH_CURRENT_CONTEXT.");
        }

        try
        {
            if (!Sdl.GlLoadLibrary(default))
            {
                throw new SdlException(
                    "Unable to load GL library: " + Sdl.GetError());
            }
            var gl = new OpenGlLibrary(
                methodName => Sdl.GlGetProcAddress("gl" + methodName));

            gl.GetIntegerv(Gl.MaxTextureSize, out var maxTextureSize);

            var version = Sdl.GetVersion();
            var major = version / 1000000;
            var minor = version / 1000 % 1000;
            var micro = version % 1000;
            var versionString = $"{major}.{minor}.{micro}";

            if (logger is not null)
            {
                var log = string.Concat(
                    "SDL version: ",
                    versionString,
                    Environment.NewLine,
                    "SDL video driver: ",
                    Sdl.GetCurrentVideoDriver(),
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

                var driverCount = Sdl.GetNumVideoDrivers();
                var drivers = Enumerable.Range(0, driverCount).Select(n => Sdl.GetVideoDriver(n));
                logger.LogDebug("Drivers: {drivers}", string.Join(", ", drivers));
            }

            return new OpenGlContext { SdlGlContextPtr = contextPtr, OpenGl = gl };
        }
        catch
        {
            Sdl.GlDestroyContext(contextPtr);
            contextPtr = default;
            throw;
        }
    }
}
