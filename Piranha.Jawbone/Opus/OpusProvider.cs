using System;
using System.Linq;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Opus;

public sealed class OpusProvider : IDisposable
{
    // TODO: Refactor to properly locate lib folder.
    private static readonly string[] LibraryPaths = new string[]
    {
        "/usr/lib/x86_64-linux-gnu/libopus.so.0",
        "/usr/lib/libopus.so"
    };

    public static OpusProvider Create(string libraryPath)
    {
        var native = NativeLibraryInterface.FromFile<IOpus>(
            libraryPath,
            methodName => NativeLibraryInterface.PascalCaseToSnakeCase("opus", methodName));
        
        return new OpusProvider(native);
    }

    public static OpusProvider Create()
    {
        var libraryPath = LibraryPaths.First(System.IO.File.Exists);
        return Create(libraryPath);
    }
    
    private readonly NativeLibraryInterface<IOpus> _native;

    private OpusProvider(NativeLibraryInterface<IOpus> native)
    {
        _native = native;
    }

    public void Dispose()
    {
        _native.Dispose();
    }

    public OpusEncoder CreateEncoder() => CreateEncoder(Default.SampleRate, Default.ChannelCount, OpusApplication.Audio);

    public OpusEncoder CreateEncoder(
        int sampleRate,
        int channelCount,
        OpusApplication application)
    {
        return new OpusEncoder(_native.Library, sampleRate, channelCount, application);
    }

    public OpusDecoder CreateDecoder() => CreateDecoder(Default.SampleRate, Default.ChannelCount);

    public OpusDecoder CreateDecoder(int sampleRate, int channelCount)
    {
        return new OpusDecoder(_native.Library, sampleRate, channelCount);
    }
}
