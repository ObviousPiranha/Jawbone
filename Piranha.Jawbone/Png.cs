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
        var header = Read<FirstHeader>(stream);
        return GetImageSize(header);
    }

    private static Point32 GetImageSize(FirstHeader header)
    {
        var expectedFileSignature = BitConverter.ToUInt64(
            [137, 80, 78, 71, 13, 10, 26, 10]);
        if (header.FileSignature != expectedFileSignature)
            throw new InvalidOperationException("Bad PNG file signature");
        var expectedChunkType = BitConverter.ToUInt32("IHDR"u8);
        if (header.ChunkType != expectedChunkType)
            throw new InvalidOperationException("Wrong chunk type");
        var result = new Point32(
            header.Width.HostValue,
            header.Height.HostValue);
        return result;
    }

    private static T Read<T>(Stream stream) where T : unmanaged
    {
        T result;
        Unsafe.SkipInit(out result);
        var bytes = MemoryMarshal.AsBytes(new Span<T>(ref result));
        var n = stream.Read(bytes);
        if (n != bytes.Length)
            throw new InvalidOperationException("Not enough bytes");
        return result;
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