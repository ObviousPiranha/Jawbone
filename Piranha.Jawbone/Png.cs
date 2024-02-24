using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

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