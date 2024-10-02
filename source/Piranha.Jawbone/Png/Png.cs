using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone.Png;

public static class Png
{
    public static Point32 GetImageSize(ReadOnlySpan<byte> bytes)
    {
        var header = MemoryMarshal.Read<FirstHeader>(bytes);
        return GetImageSize(header);
    }

    public static Point32 GetImageSize(Stream stream)
    {
        Read(stream, out FirstHeader header);
        return GetImageSize(header);
    }

    public static Point32 GetImageSize(string file)
    {
        using var stream = File.OpenRead(file);
        return GetImageSize(stream);
    }

    public static void DebugWalk(ReadOnlySpan<byte> pngData)
    {
        var reader = pngData.ToReader();

        if (!reader.TryBlit(out ulong signature))
        {
            Console.WriteLine("Missing signature!");
            return;
        }

        if (signature != ChunkHeader.Signature)
        {
            Console.WriteLine("Signature mismatch!");
            return;
        }

        if (!reader.TryBlit(out BigEndianUInt32 bigEndianLength))
        {
            Console.WriteLine("Missing chunk length.");
        }

        var crcStart = reader.Position;

        if (!reader.TrySlice(4, out var chunkType))
        {
            Console.WriteLine("Missing chunk header.");
            return;
        }

        Console.WriteLine("Chunk type: " + Encoding.UTF8.GetString(chunkType));
        var chunkTypeValue = BitConverter.ToUInt32(chunkType);

        if (chunkTypeValue != ChunkHeader.Ihdr)
        {
            Console.WriteLine("Incorrect first chunk type.");
            return;
        }

        var length = bigEndianLength.HostValue;

        if (int.MaxValue < length)
        {
            Console.WriteLine("Chunk too big.");
            return;
        }

        if (!reader.TrySlice((int)length, out var chunk))
        {
            Console.WriteLine("Not enough bytes in chunk.");
            return;
        }

        if (!reader.TryBlit(out BigEndianUInt32 bigEndianCrc))
        {
            Console.WriteLine("Missing CRC.");
            return;
        }

        var crcChunk = reader.Span.Slice(crcStart, chunk.Length + 4);
        var crc = Crc.Calculate(crcChunk);
        var word = chunk.Length == 1 ? "byte" : "bytes";

        if (crc != bigEndianCrc.HostValue)
        {
            Console.WriteLine($"Incorrect CRC for {chunk.Length} {word}. Expected {crc}. Actual {crc}.");
            return;
        }

        Console.WriteLine($"CRC ({crc}) correct for {chunk.Length} {word}.");

        while (chunkTypeValue != ChunkHeader.Iend && reader.TryBlit(out bigEndianLength))
        {
            length = bigEndianLength.HostValue;

            if (int.MaxValue < length)
            {
                Console.WriteLine("Chunk too big.");
                return;
            }

            crcStart = reader.Position;

            if (!reader.TrySlice(4, out chunkType))
            {
                Console.WriteLine("Missing chunk header.");
                return;
            }

            Console.WriteLine("Chunk type: " + Encoding.UTF8.GetString(chunkType));
            chunkTypeValue = BitConverter.ToUInt32(chunkType);

            if (!reader.TrySlice((int)length, out chunk))
            {
                Console.WriteLine("Not enough bytes in chunk.");
                return;
            }

            if (!reader.TryBlit(out bigEndianCrc))
            {
                Console.WriteLine("Missing CRC.");
                return;
            }

            crcChunk = reader.Span.Slice(crcStart, 4 + chunk.Length);
            crc = Crc.Calculate(crcChunk);

            if (crc != bigEndianCrc.HostValue)
            {
                Console.WriteLine($"Incorrect CRC. Expected {crc}. Actual {crc}.");
                return;
            }

            word = chunk.Length == 1 ? "byte" : "bytes";
            Console.WriteLine($"CRC ({crc}) correct for {chunk.Length} {word}.");
        }

        Console.WriteLine("Success!");
    }

    private static Point32 GetImageSize(in FirstHeader header)
    {
        // // http://www.libpng.org/pub/png/spec/1.2/PNG-Structure.html
        var expectedFileSignature = BitConverter.ToUInt64(
            [137, 80, 78, 71, 13, 10, 26, 10]);
        if (header.FileSignature != expectedFileSignature)
            throw new InvalidOperationException("Bad PNG file signature");
        // http://www.libpng.org/pub/png/spec/1.2/PNG-Chunks.html
        var expectedChunkType = BitConverter.ToUInt32("IHDR"u8);
        if (header.ChunkType != expectedChunkType)
            throw new InvalidOperationException("Wrong chunk type");
        var result = new Point32(
            header.Width.HostValue,
            header.Height.HostValue);
        return result;
    }

    private static void Read<T>(Stream stream, out T result) where T : unmanaged
    {
        Unsafe.SkipInit(out result);
        var bytes = MemoryMarshal.AsBytes(new Span<T>(ref result));
        var n = stream.Read(bytes);
        if (n != bytes.Length)
            throw new InvalidOperationException("Not enough bytes");
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct FirstHeader
    {
        public ulong FileSignature;
        public BigEndianInt32 Length;
        public uint ChunkType;
        public BigEndianInt32 Width;
        public BigEndianInt32 Height;
        public byte BitDepth;
        public byte ColorType;
        public byte CompressionMethod;
        public byte FilterMethod;
        public byte InterlaceMethod;
        public uint Crc;
    }
}
