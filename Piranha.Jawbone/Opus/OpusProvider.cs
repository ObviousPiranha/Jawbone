using System;
using System.Linq;

namespace Piranha.Jawbone.Opus;

public sealed class OpusProvider : IDisposable
{
    // TODO: Refactor to properly locate lib folder.
    private static readonly string[] LibraryPaths = new string[]
    {
        "opus.dll",
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

    public OpusEncoder CreateEncoder(
        int sampleRate = Default.SampleRate,
        int channelCount = Default.ChannelCount,
        OpusApplication application = Default.Application)
    {
        return new OpusEncoder(
            _native.Library,
            sampleRate,
            channelCount,
            application);
    }

    public OpusDecoder CreateDecoder(
        int sampleRate = Default.SampleRate,
        int channelCount = Default.ChannelCount)
    {
        return new OpusDecoder(
            _native.Library,
            sampleRate,
            channelCount);
    }
}
