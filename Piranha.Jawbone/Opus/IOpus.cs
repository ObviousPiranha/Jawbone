using Piranha.Jawbone.Tools;
using System;

namespace Piranha.Jawbone.Opus;

// https://opus-codec.org/docs/opus_api-1.3.1/index.html
public interface IOpus
{
    int EncoderGetSize(int channels);
    IntPtr EncoderCreate(int fs, int channels, int application, out int error);
    int EncoderInit(IntPtr st, int fs, int channels, int application);
    int Encode(IntPtr st, in short pcm, int frameSize, out byte data, int maxDataBytes);
    int EncodeFloat(IntPtr st, in float pcm, int frameSize, out byte data, int maxDataBytes);
    void EncoderDestroy(IntPtr st);
    int EncoderCtl(IntPtr st, int request, out int value);
    int EncoderCtl(IntPtr st, int request, int value);

    IntPtr DecoderCreate(int fs, int channels, out int error);
    int DecoderInit(IntPtr st, int fs, int channels);
    int Decode(IntPtr st, in byte data, int len, out short pcm, int frameSize, int decodeFec);
    int DecodeFloat(IntPtr st, in byte data, int len, out float pcm, int frameSize, int decodeFec);
    void DecoderDestroy(IntPtr st);
}
