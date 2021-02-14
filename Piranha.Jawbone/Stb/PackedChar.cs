using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Stb
{
    [StructLayout(LayoutKind.Sequential, Size = 28)]
    public struct PackedChar
    {
        public short x0;
        public short y0;
        public short x1;
        public short y1;
        public float xOff;
        public float yOff;
        public float xAdvance;
        public float xOff2;
        public float yOff2;

        public PackedChar(ReadOnlySpan<byte> bytes)
        {
            x0 = BitConverter.ToInt16(bytes);
            y0 = BitConverter.ToInt16(bytes.Slice(2));
            x1 = BitConverter.ToInt16(bytes.Slice(4));
            y1 = BitConverter.ToInt16(bytes.Slice(6));
            xOff = BitConverter.ToSingle(bytes.Slice(8));
            yOff = BitConverter.ToSingle(bytes.Slice(12));
            xAdvance = BitConverter.ToSingle(bytes.Slice(16));
            xOff2 = BitConverter.ToSingle(bytes.Slice(20));
            yOff2 = BitConverter.ToSingle(bytes.Slice(24));
        }

        public Quadrilateral<Vector2> GetCoordinates(Point32 sheetSize)
        {
            var width = (float)sheetSize.X;
            var height = (float)sheetSize.Y;
            return Quadrilateral.Create(
                new Vector2(x0 / width, y0 / height),
                new Vector2(x1 / width, y1 / height));
        }
    }
}
