using System;

namespace Piranha.Jawbone.Png;

struct ChunkHeader
{
    public static readonly ulong Signature = BitConverter.ToUInt64([137, 80, 78, 71, 13, 10, 26, 10]);
    public static readonly uint Ihdr = BitConverter.ToUInt32("IHDR"u8);
    public static readonly uint Iend = BitConverter.ToUInt32("IEND"u8);

    public BigEndianUInt32 Length;
    public uint ChunkType;
}
