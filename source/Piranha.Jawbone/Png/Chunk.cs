using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Png;

ref struct Chunk
{
    public ReadOnlySpan<byte> Data;
    public uint ChunkType;
    public uint ExpectedCrc;
    public uint ActualCrc;

    public bool DebugVerify()
    {
        var word = Data.Length == 1 ? "byte" : "bytes";

        if (ExpectedCrc != ActualCrc)
        {
            Console.WriteLine($"Incorrect CRC for {Data.Length} {word}. Expected {ExpectedCrc}. Actual {ActualCrc}.");
            return false;
        }

        var chunkName = Encoding.UTF8.GetString(
            MemoryMarshal.AsBytes(
                new ReadOnlySpan<uint>(
                    in ChunkType)));
        Console.WriteLine($"CRC ({ExpectedCrc}) correct for {chunkName}: {Data.Length} {word}.");
        return true;
    }

    public static bool TryRead(
        ref SpanReader<byte> reader,
        out Chunk chunk)
    {
        chunk = default(Chunk);
        if (!reader.TryReadBigEndianUInt32(out var length) ||
            int.MaxValue < length ||
            !reader.TryBlit(out chunk.ChunkType) ||
            !reader.TrySlice((int)length, out chunk.Data) ||
            !reader.TryReadBigEndianUInt32(out chunk.ExpectedCrc))
        {
            return false;
        }

        var crcBlock = reader.Span.Slice(
            // Rewind the CRC, data block, and chunk type
            reader.Position - 4 - chunk.Data.Length - 4,
            // Capture the chunk type and data block
            4 + chunk.Data.Length);
        chunk.ActualCrc = Crc.Calculate(crcBlock);
        return true;
    }
}
