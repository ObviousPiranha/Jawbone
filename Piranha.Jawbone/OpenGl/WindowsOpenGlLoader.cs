using Piranha.Jawbone.Extensions;
using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.OpenGl;

public delegate IntPtr ModuleLoader(string name);

public static class WindowsOpenGlLoader
{
    private static IntPtr GetGlProcAddress(
        ModuleLoader wglGetProcAddress,
        IntPtr glModulePtr,
        string functionName)
    {
        var procAddress = wglGetProcAddress(functionName);

        if (procAddress.IsInvalid())
            procAddress = NativeLibrary.GetExport(glModulePtr, functionName);

        if (procAddress.IsInvalid())
            throw new Exception("Unable to load " + functionName);

        return procAddress;
    }

    public static NativeLibraryInterface<IOpenGl> Load()
    {
        var libraryHandle = NativeLibrary.Load("opengl32");

        if (libraryHandle.IsInvalid())
            throw new Exception("Unable to load opengl32.");

        try
        {
            var wglGetProcAddressPtr = NativeLibrary.GetExport(libraryHandle, "wglGetProcAddress");

            if (wglGetProcAddressPtr.IsInvalid())
                throw new Exception("You are not loading OpenGL today.");

            var wglGetProcAddress = Marshal.GetDelegateForFunctionPointer<ModuleLoader>(wglGetProcAddressPtr);

            return NativeLibraryInterface.Create<IOpenGl>(
                "OpenGl",
                libraryHandle,
                mn => "gl" + mn,
                (handle, name) => GetGlProcAddress(wglGetProcAddress, handle, name));
        }
        catch
        {
            NativeLibrary.Free(libraryHandle);
            throw;
        }
    }
}
