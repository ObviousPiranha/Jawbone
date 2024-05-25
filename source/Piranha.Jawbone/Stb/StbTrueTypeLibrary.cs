using Piranha.Jawbone.Generation;

namespace Piranha.Jawbone.Stb;

[MapNativeFunctions]
public sealed partial class StbTrueTypeLibrary
{
    public partial int GetFontOffsetForIndex(
        ref readonly byte data,
        int index);

    public partial int InitFont(
        out StbttFontInfo fontInfo,
        ref readonly byte data,
        int offset);

    public partial int PackBegin(
        out StbttPackContext packContext,
        out byte pixels,
        int pw,
        int ph,
        int strideInBytes,
        int padding,
        nint allocContext);

    public partial void PackSetOversampling(
        ref StbttPackContext packContext,
        uint hOversample,
        uint vOversample);

    public partial int PackFontRange(
        ref StbttPackContext packContext,
        ref readonly byte fontData,
        int fontIndex,
        float fontSize,
        int firstUnicodeCharInRange,
        int numCharsInRange,
        out StbttPackedChar charDataForRange);

    public partial float ScaleForPixelHeight(
        ref readonly StbttFontInfo fontInfo,
        float height);
    public partial int GetCodepointKernAdvance(
        ref readonly StbttFontInfo fontInfo,
        int ch1,
        int ch2);
    public partial int FindGlyphIndex(
        ref readonly StbttFontInfo fontInfo,
        int unicodeCodepoint);

    public partial void PackEnd(
        ref StbttPackContext packContext);
}
