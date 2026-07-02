using Jawbone.Stb;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Sdl3;

public static class SdlExtensions
{
    private static readonly FrozenDictionary<Type, SdlGpuVertexElementFormat> TypeToAttribute = CreateTypeMapping();

    public static IServiceCollection AddAudioManager(this IServiceCollection services)
    {
        return services.AddSingleton<IAudioManager, AudioManager>();
    }

    public static CBool ThrowOnSdlFailure(this CBool result, string? message = null)
    {
        if (!result)
            SdlException.Throw(message);
        return result;
    }

    public static nint ThrowOnSdlFailure(this nint result, string? message = null)
    {
        if (result == default)
            SdlException.Throw(message);
        return result;
    }

    public static (int major, int minor, int micro) GetVersion()
    {
        var version = Sdl.GetVersion();
        return (
            version / 1000000,
            version / 1000 % 1000,
            version % 1000);
    }

    public static IEnumerable<string> EnumerateGpuDrivers()
    {
        var gpuDriverCount = Sdl.GetNumGpuDrivers();
        for (int i = 0; i < gpuDriverCount; ++i)
        {
            var gpuDriver = Sdl.GetGpuDriver(i).ToString();
            if (!string.IsNullOrWhiteSpace(gpuDriver))
                yield return gpuDriver;
        }
    }

    public static unsafe void PushVertexUniform<T>(
        nint commandBuffer,
        uint slotIndex,
        in T value) where T : unmanaged
    {
        fixed (void* p = &value)
        {
            Sdl.PushGpuVertexUniformData(
                commandBuffer,
                slotIndex,
                new(p),
                (uint)sizeof(T));
        }
    }

    public static unsafe void PushFragmentUniform<T>(
        nint commandBuffer,
        uint slotIndex,
        in T value) where T : unmanaged
    {
        fixed (void* p = &value)
        {
            Sdl.PushGpuFragmentUniformData(
                commandBuffer,
                slotIndex,
                new(p),
                (uint)sizeof(T));
        }
    }

    public static void BindGpuVertexBuffers(
        nint renderPass,
        uint firstSlot,
        params ReadOnlySpan<SdlGpuBufferBinding> bindings)
    {
        Sdl.BindGpuVertexBuffers(
            renderPass, firstSlot, bindings[0], (uint)bindings.Length);
    }

    public static void BindGpuFragmentSamplers(
        nint renderPass,
        uint firstSlot,
        params ReadOnlySpan<SdlGpuTextureSamplerBinding> bindings)
    {
        Sdl.BindGpuFragmentSamplers(
            renderPass, firstSlot, bindings[0], (uint)bindings.Length);
    }

    public static SdlGpuVertexAttribute[] GetGpuVertexAttributes<T>(uint bufferSlot = 0) where T : unmanaged
    {
        var fields = typeof(T).GetFields();
        var result = new SdlGpuVertexAttribute[fields.Length];
        for (int i = 0; i < fields.Length; ++i)
        {
            var fieldInfo = fields[i];
            var name = fieldInfo.Name;

            result[i] = new SdlGpuVertexAttribute
            {
                Location = (uint)i,
                BufferSlot = bufferSlot,
                Format = TypeToAttribute[fieldInfo.FieldType],
                Offset = (uint)Marshal.OffsetOf<T>(fieldInfo.Name)
            };
        }
        return result;
    }

    private static FrozenDictionary<Type, SdlGpuVertexElementFormat> CreateTypeMapping()
    {
        IEnumerable<KeyValuePair<Type, SdlGpuVertexElementFormat>> pairs =
        [
            new(typeof(float), SdlGpuVertexElementFormat.Float),
            new(typeof(Vector2), SdlGpuVertexElementFormat.Float2),
            new(typeof(Vector3), SdlGpuVertexElementFormat.Float3),
            new(typeof(Vector4), SdlGpuVertexElementFormat.Float4),
            new(typeof(ColorRgba32), SdlGpuVertexElementFormat.Ubyte4Norm)
        ];

        var result = pairs.ToFrozenDictionary();
        return result;
    }

    public static Point32 GetSurfaceSize(nint surfacePtr)
    {
        ref var surface = ref SdlSurface.FromPointer(surfacePtr);
        var result = new Point32(surface.W, surface.H);
        return result;
    }

    public static int RunApp(ISdlEventHandler eventHandler)
    {
        using var args = CStringArray.FromCommandLine();
        return RunApp(eventHandler, args);
    }

    public static int RunApp(ISdlEventHandler eventHandler, CStringArray args)
    {
        return Sdl.RunApp(
            args.Length,
            args.Pointer,
            (argc, argv) => Sdl.EnterAppMainCallbacks(
                argc,
                argv,
                (out nint appState, int argc, nint argv) =>
                {
                    try
                    {
                        appState = default;
                        eventHandler.OnStart();
                        return SdlAppResult.Continue;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Unsafe.SkipInit(out appState);
                        return SdlAppResult.Failure;
                    }
                },
                _ =>
                {
                    try
                    {
                        eventHandler.OnLoop();
                        return eventHandler.Running ? SdlAppResult.Continue : SdlAppResult.Success;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return SdlAppResult.Failure;
                    }
                },
                (nint appState, in SdlEvent sdlEvent) =>
                {
                    try
                    {
                        SdlEvent.Dispatch(sdlEvent, eventHandler);
                        return eventHandler.Running ? SdlAppResult.Continue : SdlAppResult.Success;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return SdlAppResult.Failure;
                    }
                },
                (_, _) =>
                {
                    try
                    {
                        eventHandler.OnStop();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }),
                default);
    }

    public static bool IsFullscreen(nint window)
    {
        var flags = Sdl.GetWindowFlags(window);
        var result = (flags & SdlWindowFlags.Fullscreen) == SdlWindowFlags.Fullscreen;
        return result;
    }

    public static CBool ToggleFullscreen(nint window)
    {
        return Sdl.SetWindowFullscreen(window, !IsFullscreen(window));
    }

    public static void BlitAndBleed(
        nint sourceSurfacePtr,
        nint destinationSurfacePtr,
        int destinationX,
        int destinationY)
    {
        const string message = "Surface pointer cannot be null.";
        if (sourceSurfacePtr == default)
            throw new ArgumentNullException(nameof(sourceSurfacePtr), message);
        if (destinationSurfacePtr == default)
            throw new ArgumentNullException(nameof(destinationSurfacePtr), message);
        var sourceSize = SdlSurface.GetSize(sourceSurfacePtr);

        Debug.Assert(0 < sourceSize.X);
        Debug.Assert(0 < sourceSize.Y);

        var w = sourceSize.X;
        var h = sourceSize.Y;
        
        var srcRect = default(SdlRect);
        var dstRect = default(SdlRect);

        dstRect.X = destinationX;
        dstRect.Y = destinationY;

        const string blitMessage = "Unable to blit surface.";
        Sdl.BlitSurface(
            sourceSurfacePtr,
            Unsafe.NullRef<SdlRect>(),
            destinationSurfacePtr,
            dstRect
            ).ThrowOnSdlFailure(blitMessage);

        // Top edge
        srcRect.X = 0;
        srcRect.Y = 0;
        srcRect.W = w;
        srcRect.H = 1;
        dstRect.X = destinationX;
        dstRect.Y = destinationY - 1;
        Sdl.BlitSurface(
            sourceSurfacePtr,
            srcRect,
            destinationSurfacePtr,
            dstRect
            ).ThrowOnSdlFailure(blitMessage);

        // Bottom edge
        srcRect.X = 0;
        srcRect.Y = h - 1;
        srcRect.W = w;
        srcRect.H = 1;
        dstRect.X = destinationX;
        dstRect.Y = destinationY + h;
        Sdl.BlitSurface(
            sourceSurfacePtr,
            srcRect,
            destinationSurfacePtr,
            dstRect
            ).ThrowOnSdlFailure(blitMessage);

        // Left edge
        srcRect.X = 0;
        srcRect.Y = 0;
        srcRect.W = 1;
        srcRect.H = h;
        dstRect.X = destinationX - 1;
        dstRect.Y = destinationY;
        Sdl.BlitSurface(
            sourceSurfacePtr,
            srcRect,
            destinationSurfacePtr,
            dstRect
            ).ThrowOnSdlFailure(blitMessage);

        // Right edge
        srcRect.X = w - 1;
        srcRect.Y = 0;
        srcRect.W = 1;
        srcRect.H = h;
        dstRect.X = destinationX + w;
        dstRect.Y = destinationY;
        Sdl.BlitSurface(
            sourceSurfacePtr,
            srcRect,
            destinationSurfacePtr,
            dstRect
            ).ThrowOnSdlFailure(blitMessage);

        // Top left corner
        srcRect.X = 0;
        srcRect.Y = 0;
        srcRect.W = 1;
        srcRect.H = 1;
        dstRect.X = destinationX - 1;
        dstRect.Y = destinationY - 1;
        Sdl.BlitSurface(
            sourceSurfacePtr,
            srcRect,
            destinationSurfacePtr,
            dstRect
            ).ThrowOnSdlFailure(blitMessage);

        // Top right corner
        srcRect.X = w - 1;
        srcRect.Y = 0;
        srcRect.W = 1;
        srcRect.H = 1;
        dstRect.X = destinationX + w;
        dstRect.Y = destinationY - 1;
        Sdl.BlitSurface(
            sourceSurfacePtr,
            srcRect,
            destinationSurfacePtr,
            dstRect
            ).ThrowOnSdlFailure(blitMessage);

        // Bottom left corner
        srcRect.X = 0;
        srcRect.Y = h - 1;
        srcRect.W = 1;
        srcRect.H = 1;
        dstRect.X = destinationX - 1;
        dstRect.Y = destinationY + h;
        Sdl.BlitSurface(
            sourceSurfacePtr,
            srcRect,
            destinationSurfacePtr,
            dstRect
            ).ThrowOnSdlFailure(blitMessage);

        // Bottom right corner
        srcRect.X = w - 1;
        srcRect.Y = h - 1;
        srcRect.W = 1;
        srcRect.H = 1;
        dstRect.X = destinationX + w;
        dstRect.Y = destinationY + h;
        Sdl.BlitSurface(
            sourceSurfacePtr,
            srcRect,
            destinationSurfacePtr,
            dstRect
            ).ThrowOnSdlFailure(blitMessage);
    }

    public static nint CreateSpriteSheet(
        string folder,
        out Point32 sheetSize,
        out Dictionary<string, Rectangle32> imageLocations,
        SdlPixelFormat pixelFormat = SdlPixelFormat.Abgr8888)
    {
        SheetBuilder.CreateSingleSheet(
            folder,
            out sheetSize,
            out imageLocations);
        var result = CreateSpriteSheet(
            folder,
            sheetSize,
            imageLocations,
            pixelFormat);
        return result;
    }

    public static nint CreateSpriteSheet(
        string folder,
        Point32 sheetSize,
        IEnumerable<KeyValuePair<string, Rectangle32>> imageLocations,
        SdlPixelFormat pixelFormat = SdlPixelFormat.Abgr8888)
    {
        var sheetSurface = Sdl.CreateSurface(sheetSize.X, sheetSize.Y, pixelFormat)
            .ThrowOnSdlFailure("Unable to create surface.");
        
        try
        {
            foreach (var pair in imageLocations)
            {
                var file = Path.Combine(folder, pair.Key);
                var spritePosition = pair.Value.Position;
                var imageSurface = Sdl.LoadPng(file)
                    .ThrowOnSdlFailure("Unable to load PNG.");
                try
                {
                    BlitAndBleed(
                        imageSurface,
                        sheetSurface,
                        spritePosition.X,
                        spritePosition.Y);
                }
                finally
                {
                    Sdl.DestroySurface(imageSurface);
                }
            }

            return sheetSurface;
        }
        catch
        {
            Sdl.DestroySurface(sheetSurface);
            throw;
        }
    }

    public static List<nint> CreateSpriteSheets(
        string folder,
        Point32 sheetSize,
        out Dictionary<string, SheetPosition> imageLocations,
        SdlPixelFormat pixelFormat = SdlPixelFormat.Abgr8888)
    {
        SheetBuilder.CreateSheets(folder, sheetSize, out imageLocations);
        var result = CreateSpriteSheets(folder, sheetSize, imageLocations, pixelFormat);
        return result;
    }

    public static List<nint> CreateSpriteSheets(
        string folder,
        Point32 sheetSize,
        IEnumerable<KeyValuePair<string, SheetPosition>> imageLocations,
        SdlPixelFormat pixelFormat = SdlPixelFormat.Abgr8888)
    {
        var surfaces = new List<nint>();

        try
        {
            foreach (var pair in imageLocations)
            {
                while (surfaces.Count <= pair.Value.SheetIndex)
                {
                    var surface = Sdl.CreateSurface(sheetSize.X, sheetSize.Y, pixelFormat)
                        .ThrowOnSdlFailure("Unable to create surface.");
                    surfaces.Add(surface);
                }
                var file = Path.Combine(folder, pair.Key);
                var spritePosition = pair.Value.Rectangle.Position;
                var imageSurface = Sdl.LoadPng(file)
                    .ThrowOnSdlFailure("Unable to load PNG.");
                try
                {
                    BlitAndBleed(
                        imageSurface,
                        surfaces[pair.Value.SheetIndex],
                        spritePosition.X,
                        spritePosition.Y);
                }
                finally
                {
                    Sdl.DestroySurface(imageSurface);
                }
            }
        }
        catch
        {
            foreach (var surface in surfaces)
                Sdl.DestroySurface(surface);
            throw;
        }

        return surfaces;
    }

    public static void SaveSurfaceAsPng(nint surfacePtr, string path)
    {
        if (surfacePtr == default)
            throw new ArgumentNullException(nameof(surfacePtr));
        ref var surface = ref SdlSurface.FromPointer(surfacePtr);
        var result = StbImageWrite.WritePng(
            path,
            surface.W,
            surface.H,
            4,
            surface.Pixels,
            surface.Pitch);
        if (result == 0)
            throw new InvalidOperationException("Failed to write PNG.");
    }

    public static CBool RenderLines(nint renderer, ReadOnlySpan<SdlFPoint> points) =>
        Sdl.RenderLines(renderer, points[0], points.Length);
    
    public static CBool RenderLines(nint renderer, ReadOnlySpan<Vector2> points)
    {
        Debug.Assert(Unsafe.SizeOf<Vector2>() == Unsafe.SizeOf<SdlFPoint>());
        var span = MemoryMarshal.Cast<Vector2, SdlFPoint>(points);
        var result = RenderLines(renderer, span);
        return result;
    }
}

