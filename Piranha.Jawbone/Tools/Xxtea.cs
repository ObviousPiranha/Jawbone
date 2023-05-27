using System;

namespace Piranha.Jawbone.Tools;

// https://en.wikipedia.org/wiki/XXTEA#Reference_code
// https://crypto.stackexchange.com/a/12997/50663
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
        return ((z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)) ^ ((sum ^ y) + (key[(int)(p & 3 ^ e)] ^ z));
    }

    private static uint Rounds(int n) => 16 + 32 / (uint)n;

    private static void Validate(Span<uint> v, ReadOnlySpan<uint> key)
    {
        if (v.Length < 2)
            throw new ArgumentException("Data must contain at least two 32-bit words", nameof(v));

        if (key.Length < 4)
            throw new ArgumentException("Key must contain four 32-bit words", nameof(key));
    }

    public static void Encrypt(Span<uint> v, ReadOnlySpan<uint> key)
    {
        Validate(v, key);
        int n = v.Length;
        uint rounds = Rounds(n);
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
        Validate(v, key);
        int n = v.Length;
        uint rounds = Rounds(n);
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

    public static int Encrypt(ReadOnlySpan<byte> input, Span<byte> output, ReadOnlySpan<uint> key)
    {
        if (input.Length < 5)
            throw new ArgumentException("Input data is too short", nameof(input));

        var expectedOutputLength = (input.Length + 3) & ~3;

        if (output.Length < expectedOutputLength)
            throw new ArgumentException("Output buffer is too short", nameof(output));

        input.CopyTo(output);
        output.Slice(input.Length, expectedOutputLength - input.Length).Clear();

        Span<uint> v;

        unsafe
        {
            fixed (void* p = output)
                v = new Span<uint>(p, expectedOutputLength / 4);
        }

        Encrypt(v, key);

        return expectedOutputLength;
    }

    public static int Decrypt(ReadOnlySpan<byte> input, Span<byte> output, ReadOnlySpan<uint> key)
    {
        if (input.Length < 8)
            throw new ArgumentException("Input data is too short", nameof(input));

        if ((input.Length & 3) != 0)
            throw new ArgumentException("Input data length must be multiple of 4", nameof(input));

        if (output.Length < input.Length)
            throw new ArgumentException("Output buffer is too short", nameof(output));

        input.CopyTo(output);

        Span<uint> v;

        unsafe
        {
            fixed (void* p = output)
                v = new Span<uint>(p, input.Length / 4);
        }

        Decrypt(v, key);

        return input.Length;
    }
}
