using Piranha.Jawbone.Generation;

namespace Piranha.Jawbone.Stb;

[MapNativeFunctions]
public sealed partial class StbVorbisLibrary
{
    public partial int DecodeMemory(
        ref readonly byte mem,
        int length,
        out int channels,
        out int sampleRate,
        out nint output);

    public partial int DecodeFilename(
        string filename,
        out int channels,
        out int sampleRate,
        out nint output);
}
