using System;

namespace Piranha.Jawbone.Png;

// http://www.libpng.org/pub/png/spec/1.2/PNG-CRCAppendix.html
static class Crc
{
    private static uint[]? s_table;

    private static uint[] MakeTable()
    {
        var result = new uint[256];

        for (int n = 0; n < 256; ++n)
        {
            var c = (uint)n;

            for (int k = 0; k < 8; ++k)
            {
                if ((c & 1) != 0)
                    c = 0xedb88320u ^ (c >> 1);
                else
                    c >>= 1;
            }

            result[n] = c;
        }

        return result;
    }

    public static uint Update(uint crc, ReadOnlySpan<byte> buffer)
    {
        s_table ??= MakeTable();
        // var hex = Array.ConvertAll(s_table, x => x.ToString("X"));

        var c = crc;
        foreach (var b in buffer)
            c = s_table[(c ^ b) & 0xff] ^ (c >> 8);
        return ~c;
    }

    public static uint Calculate(ReadOnlySpan<byte> buffer)
    {
        return Update(uint.MaxValue, buffer);
    }
}
