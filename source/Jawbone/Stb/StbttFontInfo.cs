using System.Runtime.InteropServices;

namespace Jawbone.Stb;

[StructLayout(LayoutKind.Sequential)]
public struct StbttFontInfo
{
    public nint UserData;
    public nint Data;
    public int FontStart;
    public int NumGlyphs;
    public int Loca;
    public int Head;
    public int Glyf;
    public int Hhea;
    public int Hmtx;
    public int Kern;
    public int Gpos;
    public int Svg;
    public int IndexMap;
    public int IndexToLocFormat;
    public StbttBuf Cff;
    public StbttBuf CharStrings;
    public StbttBuf Gsubrs;
    public StbttBuf Subrs;
    public StbttBuf FontDicts;
    public StbttBuf FdSelect;
}
