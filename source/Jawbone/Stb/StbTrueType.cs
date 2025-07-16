using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Stb;

#pragma warning disable CA1401

public static partial class StbTrueType
{
    public static int ThrowOnStbFailure(this int result, string? message = null)
    {
        if (result == 0)
            StbException.Throw(message);
        return result;
    }

    [LibraryImport(C.Library, EntryPoint = "stbtt_GetFontOffsetForIndex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetFontOffsetForIndex(
        in byte data,
        int index);

    [LibraryImport(C.Library, EntryPoint = "stbtt_InitFont")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int InitFont(
        out StbttFontInfo fontInfo,
        in byte data,
        int offset);

    [LibraryImport(C.Library, EntryPoint = "stbtt_PackBegin")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int PackBegin(
        out StbttPackContext packContext,
        out byte pixels,
        int pw,
        int ph,
        int strideInBytes,
        int padding,
        nint allocContext);

    [LibraryImport(C.Library, EntryPoint = "stbtt_PackSetOversampling")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void PackSetOversampling(
        ref StbttPackContext packContext,
        uint hOversample,
        uint vOversample);

    [LibraryImport(C.Library, EntryPoint = "stbtt_PackFontRange")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int PackFontRange(
        ref StbttPackContext packContext,
        in byte fontData,
        int fontIndex,
        float fontSize,
        int firstUnicodeCharInRange,
        int numCharsInRange,
        out StbttPackedChar charDataForRange);

    [LibraryImport(C.Library, EntryPoint = "stbtt_ScaleForPixelHeight")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float ScaleForPixelHeight(
        in StbttFontInfo fontInfo,
        float height);

    [LibraryImport(C.Library, EntryPoint = "stbtt_GetCodepointKernAdvance")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetCodepointKernAdvance(
        in StbttFontInfo fontInfo,
        int ch1,
        int ch2);

    [LibraryImport(C.Library, EntryPoint = "stbtt_FindGlyphIndex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int FindGlyphIndex(
        in StbttFontInfo fontInfo,
        int unicodeCodepoint);

    [LibraryImport(C.Library, EntryPoint = "stbtt_PackEnd")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void PackEnd(
        ref StbttPackContext packContext);
}

#pragma warning restore CA1401
