// Code generated at 2025-06-21T19:20:39.

namespace Jawbone.Stb;

public sealed unsafe class StbTrueTypeLibrary
{
    private readonly nint _fp_FindGlyphIndex;
    private readonly nint _fp_GetCodepointKernAdvance;
    private readonly nint _fp_GetFontOffsetForIndex;
    private readonly nint _fp_InitFont;
    private readonly nint _fp_PackBegin;
    private readonly nint _fp_PackEnd;
    private readonly nint _fp_PackFontRange;
    private readonly nint _fp_PackSetOversampling;
    private readonly nint _fp_ScaleForPixelHeight;

    public StbTrueTypeLibrary(
        System.Func<string, nint> loader)
    {
        _fp_FindGlyphIndex = loader.Invoke(nameof(FindGlyphIndex));
        _fp_GetCodepointKernAdvance = loader.Invoke(nameof(GetCodepointKernAdvance));
        _fp_GetFontOffsetForIndex = loader.Invoke(nameof(GetFontOffsetForIndex));
        _fp_InitFont = loader.Invoke(nameof(InitFont));
        _fp_PackBegin = loader.Invoke(nameof(PackBegin));
        _fp_PackEnd = loader.Invoke(nameof(PackEnd));
        _fp_PackFontRange = loader.Invoke(nameof(PackFontRange));
        _fp_PackSetOversampling = loader.Invoke(nameof(PackSetOversampling));
        _fp_ScaleForPixelHeight = loader.Invoke(nameof(ScaleForPixelHeight));
    }

    public int FindGlyphIndex(
        ref readonly StbttFontInfo fontInfo,
        int unicodeCodepoint)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, int, int
            >)_fp_FindGlyphIndex;
        fixed (void* __p_fontInfo = &fontInfo)
        {
            var __result = __fp(__p_fontInfo, unicodeCodepoint);
            return __result;
        }
    }

    public int GetCodepointKernAdvance(
        ref readonly StbttFontInfo fontInfo,
        int ch1,
        int ch2)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, int, int, int
            >)_fp_GetCodepointKernAdvance;
        fixed (void* __p_fontInfo = &fontInfo)
        {
            var __result = __fp(__p_fontInfo, ch1, ch2);
            return __result;
        }
    }

    public int GetFontOffsetForIndex(
        ref readonly byte data,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, int, int
            >)_fp_GetFontOffsetForIndex;
        fixed (void* __p_data = &data)
        {
            var __result = __fp(__p_data, index);
            return __result;
        }
    }

    public int InitFont(
        out StbttFontInfo fontInfo,
        ref readonly byte data,
        int offset)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, void*, int, int
            >)_fp_InitFont;
        fixed (void* __p_fontInfo = &fontInfo)
        fixed (void* __p_data = &data)
        {
            var __result = __fp(__p_fontInfo, __p_data, offset);
            return __result;
        }
    }

    public int PackBegin(
        out StbttPackContext packContext,
        out byte pixels,
        int pw,
        int ph,
        int strideInBytes,
        int padding,
        nint allocContext)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, void*, int, int, int, int, nint, int
            >)_fp_PackBegin;
        fixed (void* __p_packContext = &packContext)
        fixed (void* __p_pixels = &pixels)
        {
            var __result = __fp(__p_packContext, __p_pixels, pw, ph, strideInBytes, padding, allocContext);
            return __result;
        }
    }

    public void PackEnd(
        ref StbttPackContext packContext)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, void
            >)_fp_PackEnd;
        fixed (void* __p_packContext = &packContext)
        {
            __fp(__p_packContext);
        }
    }

    public int PackFontRange(
        ref StbttPackContext packContext,
        ref readonly byte fontData,
        int fontIndex,
        float fontSize,
        int firstUnicodeCharInRange,
        int numCharsInRange,
        out StbttPackedChar charDataForRange)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, void*, int, float, int, int, void*, int
            >)_fp_PackFontRange;
        fixed (void* __p_packContext = &packContext)
        fixed (void* __p_fontData = &fontData)
        fixed (void* __p_charDataForRange = &charDataForRange)
        {
            var __result = __fp(__p_packContext, __p_fontData, fontIndex, fontSize, firstUnicodeCharInRange, numCharsInRange, __p_charDataForRange);
            return __result;
        }
    }

    public void PackSetOversampling(
        ref StbttPackContext packContext,
        uint hOversample,
        uint vOversample)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, uint, uint, void
            >)_fp_PackSetOversampling;
        fixed (void* __p_packContext = &packContext)
        {
            __fp(__p_packContext, hOversample, vOversample);
        }
    }

    public float ScaleForPixelHeight(
        ref readonly StbttFontInfo fontInfo,
        float height)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, float, float
            >)_fp_ScaleForPixelHeight;
        fixed (void* __p_fontInfo = &fontInfo)
        {
            var __result = __fp(__p_fontInfo, height);
            return __result;
        }
    }
}
