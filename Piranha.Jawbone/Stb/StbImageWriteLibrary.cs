using Piranha.Jawbone.Generation;

namespace Piranha.Jawbone.Stb;

[MapNativeFunctions]
public sealed partial class StbImageWriteLibrary
{
    public partial int WritePng(
        string filename,
        int x,
        int y,
        int comp,
        nint data,
        int strideBytes);

    public partial int WritePng(
        string filename,
        int x,
        int y,
        int comp,
        ref readonly byte data,
        int strideBytes);
}