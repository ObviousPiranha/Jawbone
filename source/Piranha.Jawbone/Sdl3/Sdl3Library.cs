using Piranha.Jawbone.Generation;
using System;

// https://wiki.libsdl.org/SDL3/FrontPage
// https://wiki.libsdl.org/SDL3/CategoryAPI

namespace Piranha.Jawbone.Sdl3;

[MapNativeFunctions]
public sealed partial class Sdl3Library
{
    public static string GetFunctionName(string methodName)
    {
        if (methodName.StartsWith("Gl"))
            return string.Concat("SDL_GL_", methodName.AsSpan(2));

        return methodName switch
        {
            nameof(GetTicksNs) => "SDL_GetTicksNS",
            nameof(Free) => "SDL_free",
            _ => "SDL_" + methodName
        };
    }

    public partial int Init(SdlInit flags);
    public partial void Quit();
    public partial void Free(nint mem);
    public partial CString GetError();
    public partial nint CreateWindow(string title, int w, int h, SdlWindow flags);
    public partial void DestroyWindow(nint window);
    public partial int FillSurfaceRect(nint surface, ref readonly SdlRect rect, uint color);
    public partial void DestroySurface(nint surface);
    public partial nint GetJoysticks(out int count);
    public partial SdlBool IsGamepad(uint instanceId);
    public partial nint OpenGamepad(uint instanceId);
    public partial void CloseGamepad(nint gamepad);
    public partial nint OpenJoystick(uint instanceId);
    public partial void CloseJoystick(nint joystick);
    public partial int GetDisplayBounds(uint displayId, out SdlRect rect);
    public partial int GetDisplayUsableBounds(uint displayId, out SdlRect rect);
    public partial SdlWindow GetWindowFlags(nint window);
    public partial nint GetWindowFromID(uint windowId);
    public partial uint GetWindowID(nint window);
    public partial int GetWindowSize(nint window, out int width, out int height);
    public partial int GetWindowSizeInPixels(nint window, out int width, out int height);
    public partial uint GetDisplayForWindow(nint window);
    public partial nint GetWindowSurface(nint window);
    public partial int MaximizeWindow(nint window);
    public partial int SetRenderDrawColor(nint renderer, byte r, byte g, byte b, byte a);
    public partial int RenderClear(nint renderer);
    public partial int RenderPresent(nint renderer);
    public partial int RestoreWindow(nint window);
    public partial int SetWindowBordered(nint window, SdlBool bordered);
    public partial int SetWindowFullscreen(nint window, SdlBool fullscreen);
    public partial int SetWindowPosition(nint window, int x, int y);
    public partial int SetWindowResizable(nint window, SdlBool resizable);
    public partial int SetWindowSize(nint window, int width, int height);
    public partial int UpdateWindowSurface(nint window);
    public partial ulong GetPerformanceFrequency();
    public partial ulong GetPerformanceCounter();
    public partial SdlBool PollEvent(out SdlEvent sdlEvent);
    public partial int BlitSurface(
        nint source,
        in SdlRect sourceRectangle,
        nint destination,
        ref SdlRect destinationRectangle);
    public partial SdlBool WaitEvent(out SdlEvent sdlEvent);
    public partial uint RegisterEvents(int numEvents);
    public partial int PushEvent(ref readonly SdlEvent sdlEvent);
    public partial uint OpenAudioDevice(uint devid, ref readonly SdlAudioSpec spec);
    public partial void CloseAudioDevice(uint devid);
    public partial int PauseAudioDevice(uint dev);

    public partial int ResumeAudioDevice(uint dev);
    public partial nint CreateAudioStream(
        ref readonly SdlAudioSpec srcSpec,
        ref readonly SdlAudioSpec dstSpec);
    public partial void DestroyAudioStream(nint stream);
    public partial int BindAudioStream(uint devid, nint stream);
    public partial void UnbindAudioStream(nint stream);
    public partial int PutAudioStreamData(nint stream, ref readonly byte buffer, int length);
    public partial int PutAudioStreamData(nint stream, ref readonly short buffer, int length);
    public partial int PutAudioStreamData(nint stream, ref readonly float buffer, int length);
    public partial int FlushAudioStream(nint stream);
    public partial int GetAudioStreamAvailable(nint stream);
    public partial int GetAudioStreamData(nint stream, out byte buffer, int length);
    public partial int GetAudioStreamData(nint stream, out short buffer, int length);
    public partial int GetAudioStreamData(nint stream, out float buffer, int length);
    public partial int GlLoadLibrary(string? path = null);
    public partial void GlUnloadLibrary();
    public partial nint GlGetProcAddress(string proc);
    public partial nint GlCreateContext(nint window);
    public partial int GlDeleteContext(nint context);
    public partial int GlMakeCurrent(nint window, nint context);
    public partial int GlSetSwapInterval(int interval);
    public partial int GlSetAttribute(SdlGl attribute, int value);
    public partial void GlSwapWindow(nint window);
    public partial uint GetTicks();
    public partial ulong GetTicksNs();
    public partial nint GetCurrentDisplayMode(uint displayId);
    public partial nint GetDesktopDisplayMode(int displayId);
    public partial nint GetDisplays(out int count);
    public partial int ShowCursor();
    public partial int HideCursor();
    public partial int SetWindowTitle(nint window, string title);
    public partial int GetVersion(out SdlVersion version);
    public partial CString GetKeyName(int scanCode);
    public partial void StartTextInput();
    public partial void StopTextInput();
    public partial CString GetVideoDriver(int index);
    public partial int GetNumVideoDrivers();
    public partial CString GetCurrentVideoDriver();
    public partial SdlBool SetHint(string name, string value);
    public partial CString GetHint(string name);
    public partial CString GetAudioDeviceName(uint devid);
    public partial nint GetAudioOutputDevices(out int count);
    public partial int LockAudioStream(nint stream);
    public partial int UnlockAudioStream(nint stream);
    public partial SdlBool AudioDevicePaused(uint dev);
    public partial int GetNumCameraDrivers();
    public partial CString GetCameraDriver(int index);
}
