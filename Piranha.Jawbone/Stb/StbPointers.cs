using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Stb;

public readonly struct StbPointers
{
    private readonly nint _pointerPiranhaGetString;

    public StbPointers(nint library)
    {
        _pointerPiranhaGetString = NativeLibrary.GetExport(library, "piranha_get_string");

        if (_pointerPiranhaGetString == default)
            throw new ArgumentException("Bad function name or library handle");
    }

    public string? GetString()
    {
        unsafe
        {
            var functionPointer = (delegate*<nint>)_pointerPiranhaGetString.ToPointer();
            var result = functionPointer();
            return Marshal.PtrToStringUTF8(result);
        }
    }
}