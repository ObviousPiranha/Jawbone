using System;

namespace Piranha.Jawbone.Tools;

// https://en.wikipedia.org/wiki/XXTEA#Reference_code
public static class Xxtea
{
    private const uint Delta = 0x9e3779b9;

    private static uint Mx(
        ReadOnlySpan<uint> key,
        uint z,
        uint y,
        uint e,
        uint p,
        uint sum)
    {
        return ((z>>5^y<<2) + (y>>3^z<<4)) ^ ((sum^y) + (key[(int)(p & 3 ^ e)] ^ z));
    }

    public static void Encrypt(Span<uint> v, ReadOnlySpan<uint> key)
    {
        int n = v.Length;
        uint rounds = 6 + 52 / (uint)n;
        uint sum = 0;
        uint z = v[n - 1];

        do
        {
            sum += Delta;
            uint e = (sum >> 2) & 3;

            uint p = 0;
            uint y;
            for (; p < n - 1; ++p)
            {
                y = v[(int)(p + 1)];
                z = v[(int)p] += Mx(key, z, y, e, p, sum);
            }

            y = v[0];
            z = v[n - 1] += Mx(key, z, y, e, p, sum);
        }
        while (0 < --rounds);
    }

    public static void Decrypt(Span<uint> v, ReadOnlySpan<uint> key)
    {
        int n = v.Length;
        uint rounds = 6 + 52 / (uint)n;
        uint sum = rounds * Delta;
        uint y = v[0];

        do
        {
            uint e = (sum >> 2) & 3;
            uint p = (uint)(n - 1);
            uint z;
            for (; 0 < p; --p)
            {
                z = v[(int)(p - 1)];
                y = v[(int)p] -= Mx(key, z, y, e, p, sum);
            }

            z = v[n - 1];
            y = v[0] -= Mx(key, z, y, e, p, sum);
            sum -= Delta;
        }
        while (0 < --rounds);
    }
}