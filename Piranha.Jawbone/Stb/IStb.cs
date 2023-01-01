using System;
using System.Numerics;

namespace Piranha.Jawbone.Stb;

public interface IStb
{
    IntPtr StbiLoad(string filename, out int x, out int y, out int comp, int reqComp);
    IntPtr StbiLoadFromMemory(
        in byte buffer,
        int len,
        out int x,
        out int y,
        out int comp,
        int reqComp);
    void StbiImageFree(IntPtr imageData);

    int StbiWritePng(
        string filename,
        int x,
        int y,
        int comp,
        IntPtr data,
        int strideBytes);
    
    int StbiWritePng(
        string filename,
        int x,
        int y,
        int comp,
        byte[] data,
        int strideBytes);

    int StbVorbisDecodeMemory(
        in byte mem,
        int length,
        out int channels,
        out int sampleRate,
        out IntPtr output);
    
    int StbVorbisDecodeFilename(
        string filename,
        out int channels,
        out int sampleRate,
        out IntPtr output);
    
    int StbttGetFontOffsetForIndex(byte[] data, int index);
    
    int StbttInitFont(
        byte[] fontInfo,
        byte[] data,
        int offset);
    
    int StbttPackBegin(
        byte[] packContext,
        byte[] pixels,
        int pw,
        int ph,
        int strideInBytes,
        int padding,
        IntPtr allocContext);
    
    void StbttPackSetOversampling(
        byte[] packContext,
        uint hOversample,
        uint vOversample);
    
    int StbttPackFontRange(
        byte[] packContext,
        byte[] fontData,
        int fontIndex,
        float fontSize,
        int firstUnicodeCodePointInRange,
        int numCharsInRange,
        out PackedChar charDataForRange);
    
    float StbttScaleForPixelHeight(byte[] fontInfo, float height);
    int StbttGetCodepointKernAdvance(byte[] fontInfo, int ch1, int ch2);
    
    void StbttPackEnd(byte[] packContext);
    
    void PiranhaFree(IntPtr pointer);

    // Tests
    void PiranhaTestMatrix(in Matrix4x4 matrix);
    string? PiranhaGetString();
    string? PiranhaGetNull();
}
