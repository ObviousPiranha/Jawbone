using Piranha.Jawbone.Generation;
using System;

namespace Piranha.Jawbone.Sdl;

[MapNativeFunctions]
public sealed partial class Sdl2Library
{
    public static string GetFunctionName(string methodName)
    {
        if (methodName.StartsWith("Gl"))
            return string.Concat("SDL_GL_", methodName.AsSpan(2));

        return methodName switch
        {
            nameof(BlitSurface) => "SDL_UpperBlit",
            nameof(Free) => "SDL_free",
            _ => "SDL_" + methodName
        };
    }

    public partial int Init(SdlInit flags);
    public partial void Quit();
    public partial void Free(nint mem);
    public partial CString GetError();
    public partial nint CreateRGBSurface(
        uint flags,
        int width,
        int height,
        int depth,
        uint rmask,
        uint gmask,
        uint bmask,
        uint amask);
    public partial nint CreateRGBSurfaceFrom(
        nint pixels,
        int width,
        int height,
        int depth,
        int pitch,
        uint rmask,
        uint gmask,
        uint bmask,
        uint amask);
    public partial nint CreateWindow(string title, int x, int y, int w, int h, uint flags);
    public partial void DestroyWindow(nint window);
    public partial void FreeSurface(nint surface);
    public partial int NumJoysticks();
    public partial int IsGameController(int index);
    public partial nint GameControllerOpen(int index);
    public partial void GameControllerClose(nint gameController);
    public partial nint JoystickOpen(int index);
    public partial void JoystickClose(nint joystick);
    public partial int GetDisplayBounds(int displayIndex, out SdlRect rect);
    public partial int GetDisplayUsableBounds(int displayIndex, out SdlRect rect);
    public partial uint GetWindowFlags(nint window);
    public partial nint GetWindowFromID(uint windowId);
    public partial uint GetWindowID(nint window);
    public partial void GetWindowSize(nint window, out int width, out int height);
    public partial int GetWindowDisplayIndex(nint window);
    public partial nint GetWindowSurface(nint window);
    public partial nint CreateSoftwareRenderer(nint surface);
    public partial void MaximizeWindow(nint window);
    public partial int SetRenderDrawColor(nint renderer, byte r, byte g, byte b, byte a);
    public partial int RenderClear(nint renderer);
    public partial void RenderPresent(nint renderer);
    public partial void RestoreWindow(nint window);
    public partial void SetWindowBordered(nint window, int bordered);
    public partial int SetWindowFullscreen(nint window, uint flags);
    public partial void SetWindowPosition(nint window, int x, int y);
    public partial void SetWindowResizable(nint window, int resizable);
    public partial void SetWindowSize(nint window, int width, int height);
    public partial int UpdateWindowSurface(nint window);
    public partial ulong GetPerformanceFrequency();
    public partial ulong GetPerformanceCounter();
    public partial int PollEvent(out SdlEvent eventData);
    public partial int BlitSurface(
        nint source,
        in SdlRect sourceRectangle,
        nint destination,
        ref SdlRect destinationRectangle);
    public partial int WaitEvent(out byte eventData);
    public partial uint RegisterEvents(int numEvents);
    public partial int PushEvent(in byte eventData);
    public partial uint OpenAudioDevice(
        string? device,
        int isCapture,
        in SdlAudioSpec desired,
        out SdlAudioSpec obtained,
        SdlAudioAllowChange allowedChanges);
    public partial void CloseAudioDevice(uint dev);
    public partial void PauseAudioDevice(uint dev, int pauseOn);
    public partial int QueueAudio(uint dev, in byte data, uint len);
    public partial nint NewAudioStream(
        SdlAudioFormat sourceFormat,
        byte sourceChannels,
        int sourceRate,
        SdlAudioFormat destinationFormat,
        byte destinationChannels,
        int destinationRate);
    public partial void FreeAudioStream(nint stream);
    public partial int AudioStreamPut(nint stream, in byte buffer, int length);
    public partial int AudioStreamPut(nint stream, in short buffer, int length);
    public partial int AudioStreamFlush(nint stream);
    public partial int AudioStreamAvailable(nint stream);
    public partial int AudioStreamGet(nint stream, out byte buffer, int length);
    public partial int AudioStreamGet(nint stream, out short buffer, int length);
    public partial int AudioStreamGet(nint stream, out float buffer, int length);
    public partial int GlLoadLibrary(string? path = null);
    public partial void GlUnloadLibrary();
    public partial nint GlGetProcAddress(string proc);
    public partial nint GlCreateContext(nint window);
    public partial void GlDeleteContext(nint context);
    public partial int GlMakeCurrent(nint window, nint context);
    public partial int GlSetSwapInterval(int interval);
    public partial int GlSetAttribute(int attribute, int value);
    public partial void GlSwapWindow(nint window);
    public partial uint GetTicks();
    public partial int GetCurrentDisplayMode(int displayIndex, out SdlDisplayMode mode);
    public partial int GetDesktopDisplayMode(int displayIndex, out SdlDisplayMode mode);
    public partial int GetNumVideoDisplays();
    public partial int ShowCursor(int toggle);
    public partial void SetWindowTitle(nint window, string title);
    public partial void GetVersion(out byte v);
    public partial CString GetKeyName(int scanCode);
    public partial void StartTextInput();
    public partial void StopTextInput();
    public partial CString GetVideoDriver(int index);
    public partial int GetNumVideoDrivers();
    public partial CString GetCurrentVideoDriver();
    public partial int SetHint(string name, string value);
    public partial CString GetHint(string name);
}
