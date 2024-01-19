using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

public struct CString
{
    public nint Address;

    public override readonly string? ToString()
    {
        return Marshal.PtrToStringUTF8(Address);
    }
}
