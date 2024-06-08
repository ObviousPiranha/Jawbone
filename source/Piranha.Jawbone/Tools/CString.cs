using Piranha.Jawbone.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

public struct CString
{
    public nint Address;

    public readonly string GetStringOrDefault(string defaultValue) => ToString() ?? defaultValue;
    public readonly string GetStringOrEmpty() => ToString() ?? "";
    public unsafe readonly ReadOnlySpan<byte> AsSpan()
    {
        if (Address == default)
            return default;

        var length = 0;
        while (true)
        {
            var address = IntPtr.Add(Address, length);
            var b = Unsafe.Read<byte>(address.ToPointer());
            if (b == 0)
                break;
            ++length;
        }
        return Address.ToReadOnlySpan<byte>(length);
    }

    public override readonly string? ToString() => Marshal.PtrToStringUTF8(Address);
}
