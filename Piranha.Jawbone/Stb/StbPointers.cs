using System;
using System.Runtime.InteropServices;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Stb;

// Trying stuff out.....
// https://devblogs.microsoft.com/dotnet/improvements-in-native-code-interop-in-net-5-0/
// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/function-pointers
// https://learn.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke-source-generation
[LibraryInterface]
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