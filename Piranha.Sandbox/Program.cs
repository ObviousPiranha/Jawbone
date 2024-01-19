using Piranha.Jawbone.Sdl;
using System;
using System.Runtime.InteropServices;

namespace Piranha.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("So it begins...");
            var handle = NativeLibrary.Load("/usr/lib/x86_64-linux-gnu/libSDL2-2.0.so.0");
            var sdl = new Sdl2(methodName => NativeLibrary.GetExport(handle, Sdl2.GetFunctionName(methodName)));
            sdl.Init(SdlInit.Video);
            var keyName = sdl.GetKeyName(58);
            Console.WriteLine("Key Name: " + keyName);
            sdl.SetHint("SDL_YEAH", "nope");
            Console.WriteLine("Get Hint: " + sdl.GetHint("SDL_YEAH"));
            Console.WriteLine("Video Driver: " + sdl.GetCurrentVideoDriver());
            sdl.Quit();
            NativeLibrary.Free(handle);
            Console.WriteLine("THE END");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }
}
