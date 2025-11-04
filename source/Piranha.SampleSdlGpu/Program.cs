using Jawbone.Extensions;
using Jawbone.Sdl3;
using Jawbone.Stb;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Jawbone.SampleSdlGpu;

internal partial class Program
{
    static unsafe void RunApp()
    {
        Sdl.Init(SdlInit.Video | SdlInit.Audio | SdlInit.Events | SdlInit.Camera).ThrowOnSdlFailure("Unable to initialize SDL.");
        Sdl.SetAppMetadata("Jawbone SDL3 GPU Sample", "1.0").ThrowOnSdlFailure("Unable to set app metadata.");

        var shaderFormat =
            OperatingSystem.IsWindows() ? SdlGpuShaderFormat.Dxil :
            OperatingSystem.IsMacOS() ? SdlGpuShaderFormat.Msl :
            SdlGpuShaderFormat.Spirv;

        var window = Sdl.CreateWindow("Jawbone SDL GPU Sample", 1024, 768, SdlWindowFlags.Resizable);
        ThrowIfNull(window, "Unable to create window.");
        var device = Sdl.CreateGpuDevice(shaderFormat, true, default);
        ThrowIfNull(device, "Unable to create device.");
        Sdl.ClaimWindowForGpuDevice(device, window).ThrowOnSdlFailure("Unable to claim window for GPU device.");

        nint vertexShader;
        nint fragmentShader;

        var extension = shaderFormat switch
        {
            SdlGpuShaderFormat.Dxil => ".dxil",
            SdlGpuShaderFormat.Msl => ".msl",
            SdlGpuShaderFormat.Spirv => ".spv",
            _ => throw new InvalidOperationException("Unrecognized shader format: " + shaderFormat)
        };
        var vertexShaderSource = File.ReadAllBytes("TexturedQuad.vert" + extension);
        var entrypoint = "main\0"u8;
        fixed (void* v = entrypoint, code = vertexShaderSource)
        {
            var shaderCreateInfo = new SdlGpuShaderCreateInfo
            {
                Code = new(code),
                CodeSize = (nuint)vertexShaderSource.Length,
                Entrypoint = new(v),
                Format = shaderFormat,
                Stage = SdlGpuShaderStage.Vertex
            };
            vertexShader = Sdl.CreateGpuShader(device, shaderCreateInfo);
            ThrowIfNull(vertexShader, "Unable to create vertex shader.");
        }

        var fragmentShaderSource = File.ReadAllBytes("TexturedQuad.frag" + extension);
        fixed (void* v = entrypoint, code = fragmentShaderSource)
        {
            var shaderCreateInfo = new SdlGpuShaderCreateInfo
            {
                Code = new(code),
                CodeSize = (nuint)fragmentShaderSource.Length,
                Entrypoint = new(v),
                Format = shaderFormat,
                Stage = SdlGpuShaderStage.Fragment,
                NumSamplers = 1
            };
            fragmentShader = Sdl.CreateGpuShader(device, shaderCreateInfo);
            ThrowIfNull(fragmentShader, "Unable to create fragment shader.");
        }

        var colorTargetDescription = new SdlGpuColorTargetDescription
        {
            Format = Sdl.GetGpuSwapchainTextureFormat(device, window)
        };
        var vertexBufferDescription = new SdlGpuVertexBufferDescription
        {
            Slot = 0,
            InputRate = SdlGpuVertexInputRate.Vertex,
            InstanceStepRate = 0,
            Pitch = (uint)Unsafe.SizeOf<PositionTextureVertex>()
        };
        var vertexAttributes = new VertexAttributeArray();
        vertexAttributes[0] = new SdlGpuVertexAttribute
        {
            BufferSlot = 0,
            Format = SdlGpuVertexElementFormat.Float3,
            Location = 0,
            Offset = 0
        };
        vertexAttributes[1] = new SdlGpuVertexAttribute
        {
            BufferSlot = 0,
            Format = SdlGpuVertexElementFormat.Float2,
            Location = 1,
            Offset = (uint)Unsafe.SizeOf<float>() * 3
        };
        var pipelineCreateInfo = new SdlGpuGraphicsPipelineCreateInfo
        {
            TargetInfo = new SdlGpuGraphicsPipelineTargetInfo
            {
                NumColorTargets = 1,
                ColorTargetDescriptions = new(&colorTargetDescription)
            },
            VertexInputState = new SdlGpuVertexInputState
            {
                NumVertexBuffers = 1,
                VertexBufferDescriptions = new(&vertexBufferDescription),
                NumVertexAttributes = 2,
                VertexAttributes = new(&vertexAttributes)
            },
            PrimitiveType = SdlGpuPrimitiveType.Trianglelist,
            VertexShader = vertexShader,
            FragmentShader = fragmentShader
        };

        Console.WriteLine("Creating pipeline");
        var pipeline = Sdl.CreateGpuGraphicsPipeline(device, pipelineCreateInfo);
        ThrowIfNull(pipeline, "Unable to create pipeline.");

        Sdl.ReleaseGpuShader(device, vertexShader);
        Sdl.ReleaseGpuShader(device, fragmentShader);

        var samplerCreateInfo = new SdlGpuSamplerCreateInfo
        {
            MinFilter = SdlGpuFilter.Nearest,
            MagFilter = SdlGpuFilter.Nearest,
            MipmapMode = SdlGpuSamplerMipmapMode.Nearest,
            AddressModeU = SdlGpuSamplerAddressMode.ClampToEdge,
            AddressModeV = SdlGpuSamplerAddressMode.ClampToEdge,
            AddressModeW = SdlGpuSamplerAddressMode.ClampToEdge
        };
        var sampler = Sdl.CreateGpuSampler(device, samplerCreateInfo);

        var vertexBuffer = Sdl.CreateGpuBuffer(
            device,
            new SdlGpuBufferCreateInfo
            {
                Usage = SdlGpuBufferUsageFlags.Vertex,
                Size = (uint)(Unsafe.SizeOf<PositionTextureVertex>() * 4)
            });

        var indexBuffer = Sdl.CreateGpuBuffer(
            device,
            new SdlGpuBufferCreateInfo
            {
                Usage = SdlGpuBufferUsageFlags.Index,
                Size = (uint)(Unsafe.SizeOf<ushort>() * 6)
            });

        var imageFileBytes = File.ReadAllBytes("ravioli.png");
        var imageData = StbImage.LoadFromMemory(
            imageFileBytes[0],
            imageFileBytes.Length,
            out var width,
            out var height,
            out var comp,
            4);
        if (imageData == default)
            throw new Exception("Failed to load image.");
        var imageBytes = imageData.ToReadOnlySpan<byte>(width * height * comp);

        var texture = Sdl.CreateGpuTexture(
            device,
            new SdlGpuTextureCreateInfo
            {
                Type = SdlGpuTextureType._2D,
                Format = SdlGpuTextureFormat.R8G8B8A8Unorm,
                Width = (uint)width,
                Height = (uint)height,
                LayerCountOrDepth = 1,
                NumLevels = 1,
                Usage = SdlGpuTextureUsage.Sampler
            });

        var transferBufferSize = Unsafe.SizeOf<PositionTextureVertex>() * 4 + Unsafe.SizeOf<ushort>() * 6;
        var bufferTransferBuffer = Sdl.CreateGpuTransferBuffer(
            device,
            new SdlGpuTransferBufferCreateInfo
            {
                Usage = SdlGpuTransferBufferUsage.Upload,
                Size = (uint)transferBufferSize
            });
        int sizeOfPositions;
        int sizeOfIndices;
        {
            var mapped = Sdl.MapGpuTransferBuffer(device, bufferTransferBuffer, false);
            var transferBytes = mapped.ToSpan<byte>(transferBufferSize);
            var writer = SpanWriter.Create(transferBytes);
            writer.Blit<PositionTextureVertex>([
                new(-1, 1, 0, 0, 0),
                new(1, 1, 0, 4, 0),
                new(1, -1, 0, 4, 4),
                new(-1, -1, 0, 0, 4)]);
            sizeOfPositions = writer.Position;
            writer.Blit<ushort>([0, 1, 2, 0, 2, 3]);
            sizeOfIndices = writer.Position - sizeOfPositions;
        }
        Sdl.UnmapGpuTransferBuffer(device, bufferTransferBuffer);

        var textureTransferBuffer = Sdl.CreateGpuTransferBuffer(
            device,
            new SdlGpuTransferBufferCreateInfo
            {
                Usage = SdlGpuTransferBufferUsage.Upload,
                Size = (uint)imageBytes.Length
            });
        {
            var mapped = Sdl.MapGpuTransferBuffer(device, textureTransferBuffer, false);
            var transferBytes = mapped.ToSpan<byte>(imageBytes.Length);
            imageBytes.CopyTo(transferBytes);
        }
        Sdl.UnmapGpuTransferBuffer(device, textureTransferBuffer);

        var uploadCommandBuffer = Sdl.AcquireGpuCommandBuffer(device);
        var copyPass = Sdl.BeginGpuCopyPass(uploadCommandBuffer);

        Sdl.UploadToGpuBuffer(
            copyPass,
            new SdlGpuTransferBufferLocation
            {
                TransferBuffer = bufferTransferBuffer,
                Offset = 0
            },
            new SdlGpuBufferRegion
            {
                Buffer = vertexBuffer,
                Offset = 0,
                Size = (uint)sizeOfPositions
            },
            false);

        Sdl.UploadToGpuBuffer(
            copyPass,
            new SdlGpuTransferBufferLocation
            {
                TransferBuffer = bufferTransferBuffer,
                Offset = (uint)sizeOfPositions
            },
            new SdlGpuBufferRegion
            {
                Buffer = indexBuffer,
                Offset = 0,
                Size = (uint)sizeOfIndices
            },
            false);

        Sdl.UploadToGpuTexture(
            copyPass,
            new SdlGpuTextureTransferInfo
            {
                TransferBuffer = textureTransferBuffer,
                Offset = 0
            },
            new SdlGpuTextureRegion
            {
                Texture = texture,
                W = (uint)width,
                H = (uint)height,
                D = 1
            },
            false);

        Sdl.EndGpuCopyPass(copyPass);
        Sdl.SubmitGpuCommandBuffer(uploadCommandBuffer);
        Sdl.ReleaseGpuTransferBuffer(device, bufferTransferBuffer);
        Sdl.ReleaseGpuTransferBuffer(device, textureTransferBuffer);

        Console.WriteLine("Initialization complete!");
        {
            var version = Sdl.GetVersion();
            var major = version / 1000000;
            var minor = version / 1000 % 1000;
            var micro = version % 1000;

            var gpuShaderFormats = Sdl.GetGpuShaderFormats(device);
            var formats = string.Join(", ", SdlGpuShaderFormat.EnumerateFormatNames(gpuShaderFormats));

            var gpuDriverCount = Sdl.GetNumGpuDrivers();
            var gpuDrivers = new string[gpuDriverCount];
            for (int i = 0; i < gpuDriverCount; ++i)
                gpuDrivers[i] = Sdl.GetGpuDriver(i).ToString() ?? "";

            Console.WriteLine($"SDL version: {major}.{minor}.{micro}");
            Console.WriteLine($"Available GPU device driver: {string.Join(", ", gpuDrivers)}");
            Console.WriteLine($"SDL video driver: {Sdl.GetCurrentVideoDriver()}");
            Console.WriteLine($"SDL GPU device driver: {Sdl.GetGpuDeviceDriver(device)}");
            Console.WriteLine($"Available GPU shader formats: {formats}");
        }

        var now = Stopwatch.GetTimestamp();
        var sdlEvent = default(SdlEvent);
        var quit = false;
        while (!quit)
        {
            while (Sdl.PollEvent(out sdlEvent))
            {
                switch (sdlEvent.Type)
                {
                    case SdlEventType.WindowExposed:
                        // Draw();
                        break;
                    case SdlEventType.MouseButtonDown:
                        Console.WriteLine($"click ({sdlEvent.Button.X}, {sdlEvent.Button.Y})");
                        break;
                    case SdlEventType.Quit:
                        quit = true;
                        break;
                }
            }

            Draw();
            Thread.Sleep(1);
        }

        Console.WriteLine("Shutting down...");

        Sdl.ReleaseGpuGraphicsPipeline(device, pipeline);
        Sdl.ReleaseGpuBuffer(device, vertexBuffer);
        Sdl.ReleaseGpuBuffer(device, indexBuffer);
        Sdl.ReleaseGpuTexture(device, texture);
        Sdl.ReleaseGpuSampler(device, sampler);
        Sdl.ReleaseWindowFromGpuDevice(device, window);
        Sdl.DestroyWindow(window);
        Sdl.DestroyGpuDevice(device);

        void Draw()
        {
            var commandBuffer = Sdl.AcquireGpuCommandBuffer(device);
            ThrowIfNull(commandBuffer, "Unable to acquire command buffer.");
            Sdl.WaitAndAcquireGpuSwapchainTexture(
                commandBuffer,
                window,
                out var swapchainTexture,
                out var width,
                out var height).ThrowOnSdlFailure("Failed to acquire swapchain texture.");

            if (swapchainTexture != default)
            {
                var colorTargetInfo = new SdlGpuColorTargetInfo
                {
                    Texture = swapchainTexture,
                    ClearColor = new SdlFColor { B = 0.5f, A = 1f },
                    LoadOp = SdlGpuLoadOp.Clear,
                    StoreOp = SdlGpuStoreOp.Store
                };

                var renderPass = Sdl.BeginGpuRenderPass(commandBuffer, colorTargetInfo, 1, default);
                Sdl.BindGpuGraphicsPipeline(renderPass, pipeline);
                Sdl.BindGpuVertexBuffers(
                    renderPass,
                    0,
                    new SdlGpuBufferBinding
                    {
                        Buffer = vertexBuffer,
                        Offset = 0
                    },
                    1);
                Sdl.BindGpuIndexBuffer(
                    renderPass,
                    new SdlGpuBufferBinding
                    {
                        Buffer = indexBuffer,
                        Offset = 0
                    },
                    SdlGpuIndexElementSize._16Bit);
                Sdl.BindGpuFragmentSamplers(
                    renderPass,
                    0,
                    new SdlGpuTextureSamplerBinding
                    {
                        Texture = texture,
                        Sampler = sampler
                    },
                    1);
                Sdl.DrawGpuIndexedPrimitives(renderPass, 6, 1, 0, 0, 0);
                Sdl.EndGpuRenderPass(renderPass);
            }
            
            Sdl.SubmitGpuCommandBuffer(commandBuffer);
        }
    }

    static void ThrowIfNull(nint ptr, string message)
    {
        if (ptr != 0)
            return;

        var error = Sdl.GetError().ToString();
        throw new SdlException(message + " - " + error);
    }

    private static void Main(string[] args)
    {
        try
        {
            RunApp();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }

    [InlineArray(2)]
    private struct VertexAttributeArray
    {
        private SdlGpuVertexAttribute _v;
    }
}