using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

#pragma warning disable CA1401

public static partial class C
{
    public const string Library = "JawboneNative";
    public static ImmutableArray<string> SystemLibs { get; } = ImmutableArray.Create(["libc", "kernel32", "ws2_32"]);

    public static string GetLibraryName()
    {
        if (OperatingSystem.IsWindows())
            return "JawboneNative.dll";
        else if (OperatingSystem.IsMacOS())
            return "libJawboneNative.dylib";
        else if (OperatingSystem.IsLinux())
            return "libJawboneNative.so";

        throw new PlatformNotSupportedException();
    }

    [LibraryImport(Library, EntryPoint = "jawbone_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Free(nint ptr);
}

#pragma warning restore CA1401
