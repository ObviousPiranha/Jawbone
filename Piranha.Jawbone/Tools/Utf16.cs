using System;

namespace Piranha.Jawbone;

public static class Utf16
{
    private const string LowerHex = "0123456789abcdef";

    public static char GetHighHexDigit(byte b) => LowerHex[b >> 4];
    public static char GetLowHexDigit(byte b) => LowerHex[b & 0xf];
}
