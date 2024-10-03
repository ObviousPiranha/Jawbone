using System;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

public static partial class C
{
    public static ImmutableArray<string> SystemLibs => ImmutableArray.Create(["libc", "kernel32", "ws2_32"]);

    public static void Free(nint ptr)
    {
        if (OperatingSystem.IsWindows())
            WindowsFree(ptr);
        else if (OperatingSystem.IsMacOS())
            UnixFree(ptr);
        else if (OperatingSystem.IsLinux())
            UnixFree(ptr);
        else
            throw new PlatformNotSupportedException();
    }

    [LibraryImport("kernel32", EntryPoint = "free")]
    private static partial void WindowsFree(nint ptr);

    [LibraryImport("libc", EntryPoint = "free")]
    private static partial void UnixFree(nint ptr);
}
