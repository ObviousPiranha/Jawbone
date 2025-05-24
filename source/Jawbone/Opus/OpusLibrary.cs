using Jawbone.Generation;

namespace Jawbone.Opus;

// https://opus-codec.org/docs/opus_api-1.3.1/index.html
[MapNativeFunctions]
public sealed partial class OpusLibrary
{
    public partial int EncoderGetSize(int channels);
    public partial nint EncoderCreate(int fs, int channels, int application, out int error);
    public partial int EncoderInit(nint st, int fs, int channels, int application);
    public partial int Encode(nint st, in short pcm, int frameSize, out byte data, int maxDataBytes);
    public partial int EncodeFloat(nint st, in float pcm, int frameSize, out byte data, int maxDataBytes);
    public partial void EncoderDestroy(nint st);
    public partial int EncoderCtl(nint st, int request, out int value);
    public partial int EncoderCtl(nint st, int request, int value);

    public partial nint DecoderCreate(int fs, int channels, out int error);
    public partial int DecoderInit(nint st, int fs, int channels);
    public partial int Decode(nint st, in byte data, int len, out short pcm, int frameSize, int decodeFec);
    public partial int DecodeFloat(nint st, in byte data, int len, out float pcm, int frameSize, int decodeFec);
    public partial void DecoderDestroy(nint st);
}
