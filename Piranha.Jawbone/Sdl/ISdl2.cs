namespace Piranha.Jawbone.Sdl;

public interface ISdl2
{
    int Init(uint flags);
    void Quit();
    string GetError();
    nint CreateRGBSurface(
        uint flags,
        int width,
        int height,
        int depth,
        uint rmask,
        uint gmask,
        uint bmask,
        uint amask);
    nint CreateRGBSurfaceFrom(
        nint pixels,
        int width,
        int height,
        int depth,
        int pitch,
        uint rmask,
        uint gmask,
        uint bmask,
        uint amask);
    nint CreateWindow(string title, int x, int y, int w, int h, uint flags);
    void DestroyWindow(nint window);
    void FreeSurface(nint surface);
    int GetDisplayBounds(int displayIndex, out SdlRect rect);
    int GetDisplayUsableBounds(int displayIndex, out SdlRect rect);
    uint GetWindowFlags(nint window);
    nint GetWindowFromID(uint windowId);
    uint GetWindowID(nint window);
    void GetWindowSize(nint window, out int width, out int height);
    int GetWindowDisplayIndex(nint window);
    nint GetWindowSurface(nint window);
    nint CreateSoftwareRenderer(nint surface);
    void MaximizeWindow(nint window);
    int SetRenderDrawColor(nint renderer, byte r, byte g, byte b, byte a);
    int RenderClear(nint renderer);
    void RenderPresent(nint renderer);
    void RestoreWindow(nint window);
    void SetWindowBordered(nint window, int bordered);
    int SetWindowFullscreen(nint window, uint flags);
    void SetWindowPosition(nint window, int x, int y);
    void SetWindowResizable(nint window, int resizable);
    void SetWindowSize(nint window, int width, int height);
    int UpdateWindowSurface(nint window);
    ulong GetPerformanceFrequency();
    ulong GetPerformanceCounter();
    int PollEvent(out byte eventData);
    int UpperBlit(
        nint source,
        in SdlRect sourceRectangle,
        nint destination,
        ref SdlRect destinationRectangle);
    int WaitEvent(out byte eventData);
    uint RegisterEvents(int numEvents);
    int PushEvent(in byte eventData);
    uint OpenAudioDevice(
        string? device,
        int isCapture,
        in AudioSpec desired,
        out AudioSpec obtained,
        int allowedChanges);
    void CloseAudioDevice(uint dev);
    void PauseAudioDevice(uint dev, int pauseOn);
    int QueueAudio(uint dev, in byte data, uint len);
    nint NewAudioStream(
        ushort sourceFormat,
        byte sourceChannels,
        int sourceRate,
        ushort destinationFormat,
        byte destinationChannels,
        int destinationRate);
    void FreeAudioStream(nint stream);
    int AudioStreamPut(nint stream, in byte buffer, int length);
    int AudioStreamPut(nint stream, in short buffer, int length);
    int AudioStreamFlush(nint stream);
    int AudioStreamAvailable(nint stream);
    int AudioStreamGet(nint stream, out byte buffer, int length);
    int AudioStreamGet(nint stream, out short buffer, int length);
    int AudioStreamGet(nint stream, out float buffer, int length);
    int GlLoadLibrary(string? path);
    void GlUnloadLibrary();
    nint GlGetProcAddress(string proc);
    nint GlCreateContext(nint window);
    void GlDeleteContext(nint context);
    int GlMakeCurrent(nint window, nint context);
    int GlSetSwapInterval(int interval);
    int GlSetAttribute(int attribute, int value);
    void GlSwapWindow(nint window);
    uint GetTicks();
    int GetCurrentDisplayMode(int displayIndex, out SdlDisplayMode mode);
    int GetDesktopDisplayMode(int displayIndex, out SdlDisplayMode mode);
    int GetNumVideoDisplays();
    int ShowCursor(int toggle);
    void SetWindowTitle(nint window, string title);
    void GetVersion(out byte v);
    string GetKeyName(int scanCode);
    void StartTextInput();
    void StopTextInput();
}
