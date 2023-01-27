using System;

namespace Piranha.Jawbone;

public readonly struct WavHeader
{
    private static readonly byte[] _keys;

    static WavHeader()
    {
        var keys = "RIFFWAVEfmt data";
        _keys = new byte[keys.Length];

        for (int i = 0; i < keys.Length; ++i)
            _keys[i] = (byte)keys[i];
    }

    public int ChunkSize { get; }
    public int SampleRate { get; }
    public int ByteRate { get; }
    public short Format { get; }
    public short Channels { get; }
    public short BitsPerSample { get; }
    public short Excess { get; }
    public int DataOffset { get; }

    public WavHeader(ReadOnlySpan<byte> data) : this()
    {
        var keySpan = _keys.AsSpan();

        if (data.Length > 44 &&
            data.StartsWith(keySpan.Slice(0, 4)) &&
            data.Slice(8).StartsWith(keySpan.Slice(4, 4)))
        {
            if (data.Slice(12).StartsWith(keySpan.Slice(8, 4)))
            {
                Format = BitConverter.ToInt16(data.Slice(20));
                Channels = BitConverter.ToInt16(data.Slice(22));
                SampleRate = BitConverter.ToInt32(data.Slice(24));
                ByteRate = BitConverter.ToInt32(data.Slice(28));
                BitsPerSample = BitConverter.ToInt16(data.Slice(34));

                if (data.Slice(36).StartsWith(keySpan.Slice(12, 4)))
                {
                    ChunkSize = BitConverter.ToInt32(data.Slice(40));
                    DataOffset = 44;
                }
            }
        }
    }
}
