using Piranha.Jawbone.Generation;
using System;

namespace Piranha.Jawbone.Sdl2;

// https://wiki.libsdl.org/SDL2/FrontPage
// https://wiki.libsdl.org/SDL2/CategoryAPI

[MapNativeFunctions]
public sealed partial class Sdl2Library
{
    private const string Vulkan = "Vulkan";

    public static string GetFunctionName(string methodName)
    {
        if (methodName.StartsWith("Gl"))
            return string.Concat("SDL_GL_", methodName.AsSpan(2));

        if (methodName.StartsWith(Vulkan))
            return string.Concat("SDL_Vulkan_", methodName.AsSpan(Vulkan.Length));

        return methodName switch
        {
            nameof(BlitSurface) => "SDL_UpperBlit",
            nameof(Free) => "SDL_free",
            _ => "SDL_" + methodName
        };
    }

    public partial int VulkanCreateSurface(nint window, nint instance, out nint surface);

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
    public partial nint CreateRenderer(nint window, int index, SdlRendererFlags flags);
    public partial void DestroyRenderer(nint renderer);
    public partial int RenderFillRect(nint renderer, ref readonly SdlRect rect);
    public partial int FillRect(nint surface, ref readonly SdlRect rect, uint color);
    public partial int GetRendererOutputSize(nint renderer, out int w, out int h);
    public partial void FreeSurface(nint surface);
    public partial int NumJoysticks();
    public partial int IsGameController(int index);
    public partial int GameControllerAddMapping(string mappingString);
    // TODO: SDL_GameControllerAddMappingsFromRW
    public partial void GameControllerClose(nint gameController);
    public partial int GameControllerEventState(int state);
    public partial nint GameControllerFromInstanceID(int joyid);
    public partial nint GameControllerFromPlayerIndex(int playerIndex);
    public partial SdlBool GameControllerGetAttached(nint gamecontroller);
    public partial short GameControllerGetAxis(nint gamecontroller, SdlControllerAxis axis);
    public partial SdlControllerAxis GameControllerGetAxisFromString(string str);
    public partial SdlGameControllerButtonBind GameControllerGetBindForAxis(nint gamecontroller, SdlControllerAxis axis);
    public partial SdlGameControllerButtonBind GameControllerGetBindForButton(nint gamecontroller, SdlControllerButton button);
    public partial byte GameControllerGetButton(nint gamecontroller, SdlControllerButton button);
    public partial SdlControllerButton GameControllerGetButtonFromString(string str);
    public partial ushort GameControllerGetFirmwareVersion(nint gamecontroller);
    public partial nint GameControllerGetJoystick(nint gamecontroller);
    public partial int GameControllerGetNumTouchpadFingers(nint gamecontroller, int touchpad);
    public partial int GameControllerGetNumTouchpads(nint gamecontroller);
    public partial int GameControllerGetPlayerIndex(nint gamecontroller);
    public partial ushort GameControllerGetProduct(nint gamecontroller);
    public partial ushort GameControllerGetProductVersion(nint gamecontroller);
    public partial int GameControllerGetSensorData(nint gamecontroller, SdlSensorType type, out float data, int numValues);
    public partial float GameControllerGetSensorDataRate(nint gamecontroller, SdlSensorType type);
    public partial int GameControllerGetSensorDataWithTimestamp(nint gamecontroller, SdlSensorType type, out ulong timestamp, out float data, int numValues);
    public partial CString GameControllerGetSerial(nint gamecontroller);
    public partial ulong GameControllerGetSteamHandle(nint gamecontroller);
    public partial CString GameControllerGetStringForAxis(SdlControllerAxis axis);
    public partial CString GameControllerGetStringForButton(SdlControllerButton button);
    public partial int GameControllerGetTouchpadFinger(nint gamecontroller, int touchpad, int finger, out byte state, out float x, out float y, out float pressure);
    public partial SdlControllerType GameControllerGetType(nint gamecontroller);
    public partial ushort GameControllerGetVendor(nint gamecontroller);
    public partial SdlBool GameControllerHasAxis(nint gamecontroller, SdlControllerAxis axis);
    public partial SdlBool GameControllerHasButton(nint gamecontroller, SdlControllerButton button);
    public partial SdlBool GameControllerHasLED(nint gamecontroller);
    public partial SdlBool GameControllerHasRumble(nint gamecontroller);
    public partial SdlBool GameControllerHasRumbleTriggers(nint gamecontroller);
    public partial SdlBool GameControllerHasSensor(nint gamecontroller, SdlSensorType type);
    public partial SdlBool GameControllerIsSensorEnabled(nint gamecontroller, SdlSensorType type);
    public partial CString GameControllerMapping(nint gamecontroller);
    public partial CString GameControllerMappingForDeviceIndex(int joystickIndex);
    public partial CString GameControllerMappingForGUID(Guid guid);
    public partial CString GameControllerMappingForIndex(int mappingIndex);
    public partial CString GameControllerName(nint gameController);
    public partial int GameControllerNumMappings();
    public partial nint GameControllerOpen(int index);
    public partial CString GameControllerPath(nint gamecontroller);
    public partial CString GameControllerPathForIndex(int joystickIndex);
    public partial int GameControllerRumble(nint gameController, ushort lowFrequencyRumble, ushort highFrequencyRumble, uint durationMs);
    public partial int GameControllerRumbleTriggers(nint gamecontroller, ushort leftRumble, ushort rightRumble, uint durationMs);
    public partial int GameControllerSendEffect(nint gamecontroller, ref readonly byte data, int size);
    public partial int GameControllerSetLED(nint gamecontroller, byte red, byte green, byte blue);
    public partial void GameControllerSetPlayerIndex(nint gamecontroller, int playerIndex);
    public partial int GameControllerSetSensorEnabled(nint gamecontroller, SdlSensorType type, SdlBool enabled);
    public partial SdlControllerType GameControllerTypeForIndex(int joystickIndex);
    public partial void GameControllerUpdate();
    public partial int JoystickAttachVirtual(SdlJoystickType type, int naxes, int nbuttons, int nhats);

    // TODO: SDL_JoystickAttachVirtualEx

    public partial int JoystickDetachVirtual(int deviceIndex);
    public partial int JoystickEventState(int state);
    public partial nint JoystickFromInstanceID(uint instanceId);
    public partial nint JoystickFromPlayerIndex(int playerIndex);
    public partial SdlBool JoystickGetAttached(nint joystick);
    public partial short JoystickGetAxis(nint joystick, int axis);
    public partial SdlBool JoystickGetAxisInitialState(nint joystick, int axis, out short state);
    public partial int JoystickGetBall(nint joystick, int ball, out int dx, out int dy);
    public partial byte JoystickGetButton(nint joystick, int button);
    public partial Guid JoystickGetDeviceGUID(int deviceIndex);
    public partial uint JoystickGetDeviceInstanceID(int deviceIndex);
    public partial int JoystickGetDevicePlayerIndex(int deviceIndex);
    public partial ushort JoystickGetDeviceProduct(int deviceIndex);
    public partial ushort JoystickGetDeviceProductVersion(int deviceIndex);
    public partial SdlJoystickType JoystickGetDeviceType(int deviceIndex);
    public partial ushort JoystickGetDeviceVendor(int deviceIndex);
    public partial ushort JoystickGetFirmwareVersion(nint joystick);
    public partial Guid JoystickGetGUID(nint joystick);
    public partial Guid JoystickGetGUIDFromString(string pchGuid);
    public partial void JoystickGetGUIDString(Guid guid, out byte guidBuffer, int byteCount);
    public partial byte JoystickGetHat(nint joystick, int hat);
    public partial int JoystickGetPlayerIndex(nint joystick);
    public partial ushort JoystickGetProduct(nint joystick);
    public partial ushort JoystickGetProductVersion(nint joystick);
    public partial CString JoystickGetSerial(nint joystick);
    public partial SdlJoystickType JoystickGetType(nint joystick);
    public partial ushort JoystickGetVendor(nint joystick);
    public partial SdlBool JoystickHasLED(nint joystick);
    public partial SdlBool JoystickHasRumble(nint joystick);
    public partial SdlBool JoystickHasRumbleTriggers(nint joystick);
    public partial uint JoystickInstanceID(nint joystick);
    public partial int JoystickIsHaptic(nint joystick);
    public partial SdlBool JoystickIsVirtual(int deviceIndex);
    public partial CString JoystickName(nint joystick);
    public partial CString JoystickNameForIndex(int deviceIndex);
    public partial int JoystickNumAxes(nint joystick);
    public partial int JoystickNumBalls(nint joystick);
    public partial int JoystickNumButtons(nint joystick);
    public partial int JoystickNumHats(nint joystick);
    public partial nint JoystickOpen(int index);
    public partial CString JoystickPath(nint joystick);
    public partial CString JoystickPathForIndex(int deviceIndex);
    public partial void JoystickClose(nint joystick);
    public partial int JoystickRumble(nint joystick, ushort lowFrequencyRumble, ushort highFrequencyRumble, uint durationMs);
    public partial int JoystickRumbleTriggers(nint joystick, ushort leftRumble, ushort rightRumble, uint durationMs);
    public partial int JoystickSendEffect(nint joystick, ref readonly byte data, int size);
    public partial int JoystickSetLED(nint joystick, byte red, byte green, byte blue);
    public partial void JoystickSetPlayerIndex(nint joystick, int playerIndex);
    public partial int JoystickSetVirtualAxis(nint joystick, int axis, short value);
    public partial int JoystickSetVirtualButton(nint joystick, int button, byte value);
    public partial int JoystickSetVirtualHat(nint joystick, int hat, byte value);
    public partial void JoystickUpdate();
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
    public partial int WaitEvent(out SdlEvent eventData);
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
    public partial void ClearQueuedAudio(uint dev);
    public partial int QueueAudio(uint dev, ref readonly byte data, uint len);
    public partial int QueueAudio(uint dev, ref readonly float data, uint len);
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
    public partial int GetNumAudioDevices(int isCapture);
    public partial CString GetAudioDeviceName(int index, int isCapture);
    public partial void LockAudioDevice(uint dev);
    public partial void UnlockAudioDevice(uint dev);
    public partial SdlAudioStatus GetAudioDeviceStatus(uint dev);
}
