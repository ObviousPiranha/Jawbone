using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

public struct CString
{
    public nint Address;

    public readonly string GetStringOrDefault(string defaultValue) => ToString() ?? defaultValue;
    public readonly string GetStringOrEmpty() => ToString() ?? "";

    public override readonly string? ToString()
    {
        return Marshal.PtrToStringUTF8(Address);
    }
}
