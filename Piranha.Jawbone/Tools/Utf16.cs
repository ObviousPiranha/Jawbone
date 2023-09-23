using System;

namespace Piranha.Jawbone;

public static class Utf16
{
    public static char GetHighHexDigit(byte b) => Hex.Lower[b >> 4];
    public static char GetLowHexDigit(byte b) => Hex.Lower[b & 0xf];
}
