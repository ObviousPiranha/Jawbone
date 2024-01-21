using Piranha.Jawbone.Generation;

namespace Piranha.Jawbone.Stb;

[MapNativeFunctions]
public sealed partial class StbImageLibrary
{
    public partial nint Load(
        string filename,
        out int x,
        out int y,
        out int comp,
        int reqComp);
    public partial nint LoadFromMemory(
        ref readonly byte buffer,
        int len,
        out int x,
        out int y,
        out int comp,
        int reqComp);
    public partial void ImageFree(
        nint imageData);
}
