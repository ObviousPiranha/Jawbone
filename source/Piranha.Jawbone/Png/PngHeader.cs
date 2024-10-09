using System;

namespace Piranha.Jawbone.Png;

struct PngHeader
{
    public int Width;
    public int Height;
    public int BitDepth;
    public int ColorType;
    public int CompressionMethod;
    public int FilterMethod;
    public int InterlaceMethod;

    public readonly bool IsValid
    {
        get
        {
            if (Width < 1 || Height < 1)
                return false;

            ReadOnlySpan<int> ct0 = [1, 2, 4, 8, 16];
            ReadOnlySpan<int> ct3 = [1, 2, 4, 8];

            var isValid = ColorType switch
            {
                0 => ct0.Contains(BitDepth),
                2 or 4 or 6 => BitDepth == 8 || BitDepth == 16,
                3 => ct3.Contains(BitDepth),
                _ => false
            };

            if (!isValid)
                return false;

            if (CompressionMethod != 0 || FilterMethod != 0)
                return false;

            if (InterlaceMethod != 0 && InterlaceMethod != 1)
                return false;

            return true;
        }
    }

    public static PngHeader FromChunk(ReadOnlySpan<byte> chunk)
    {
        var reader = chunk.ToReader();
        var result = new PngHeader
        {
            Width = reader.ReadBigEndianInt32(),
            Height = reader.ReadBigEndianInt32(),
            BitDepth = reader.Take(),
            ColorType = reader.Take(),
            CompressionMethod = reader.Take(),
            FilterMethod = reader.Take(),
            InterlaceMethod = reader.Take()
        };

        return result;
    }
}
