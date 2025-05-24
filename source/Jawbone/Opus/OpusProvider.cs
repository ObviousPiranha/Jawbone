using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Jawbone.Opus;

public sealed class OpusProvider : IDisposable
{
    // TODO: Refactor to properly locate lib folder.
    private static readonly string[] LibraryPaths = new string[]
    {
        "opus.dll",
        "/usr/lib/x86_64-linux-gnu/libopus.so.0",
        "/usr/lib/libopus.so"
    };

    public static OpusProvider Create()
    {
        var libraryPath = LibraryPaths.First(System.IO.File.Exists);
        return new OpusProvider(libraryPath);
    }

    private readonly nint _handle;
    private readonly OpusLibrary _library;

    public OpusProvider(string path)
    {
        _handle = NativeLibrary.Load(path);
        _library = new OpusLibrary(
            methodName => NativeLibrary.GetExport(
                _handle, PascalCase.ToSnakeCase("opus", methodName)));
    }

    public void Dispose()
    {
        NativeLibrary.Free(_handle);
    }

    public OpusEncoder CreateEncoder(
        int sampleRate = Default.SampleRate,
        int channelCount = Default.ChannelCount,
        OpusApplication application = Default.Application)
    {
        return new OpusEncoder(
            _library,
            sampleRate,
            channelCount,
            application);
    }

    public OpusDecoder CreateDecoder(
        int sampleRate = Default.SampleRate,
        int channelCount = Default.ChannelCount)
    {
        return new OpusDecoder(
            _library,
            sampleRate,
            channelCount);
    }
}
