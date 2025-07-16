// Code generated at 2025-06-21T19:12:10.

namespace Jawbone.Sdl2;

public sealed unsafe class Sdl2Library
{
    private readonly nint _fp_AudioStreamAvailable;
    private readonly nint _fp_AudioStreamFlush;
    private readonly nint _fp_AudioStreamGet;
    private readonly nint _fp_AudioStreamPut;
    private readonly nint _fp_BlitSurface;
    private readonly nint _fp_ClearQueuedAudio;
    private readonly nint _fp_CloseAudioDevice;
    private readonly nint _fp_CreateRenderer;
    private readonly nint _fp_CreateRGBSurface;
    private readonly nint _fp_CreateRGBSurfaceFrom;
    private readonly nint _fp_CreateSoftwareRenderer;
    private readonly nint _fp_CreateWindow;
    private readonly nint _fp_DestroyRenderer;
    private readonly nint _fp_DestroyWindow;
    private readonly nint _fp_FillRect;
    private readonly nint _fp_Free;
    private readonly nint _fp_FreeAudioStream;
    private readonly nint _fp_FreeSurface;
    private readonly nint _fp_GameControllerAddMapping;
    private readonly nint _fp_GameControllerClose;
    private readonly nint _fp_GameControllerEventState;
    private readonly nint _fp_GameControllerFromInstanceID;
    private readonly nint _fp_GameControllerFromPlayerIndex;
    private readonly nint _fp_GameControllerGetAttached;
    private readonly nint _fp_GameControllerGetAxis;
    private readonly nint _fp_GameControllerGetAxisFromString;
    private readonly nint _fp_GameControllerGetBindForAxis;
    private readonly nint _fp_GameControllerGetBindForButton;
    private readonly nint _fp_GameControllerGetButton;
    private readonly nint _fp_GameControllerGetButtonFromString;
    private readonly nint _fp_GameControllerGetFirmwareVersion;
    private readonly nint _fp_GameControllerGetJoystick;
    private readonly nint _fp_GameControllerGetNumTouchpadFingers;
    private readonly nint _fp_GameControllerGetNumTouchpads;
    private readonly nint _fp_GameControllerGetPlayerIndex;
    private readonly nint _fp_GameControllerGetProduct;
    private readonly nint _fp_GameControllerGetProductVersion;
    private readonly nint _fp_GameControllerGetSensorData;
    private readonly nint _fp_GameControllerGetSensorDataRate;
    private readonly nint _fp_GameControllerGetSensorDataWithTimestamp;
    private readonly nint _fp_GameControllerGetSerial;
    private readonly nint _fp_GameControllerGetSteamHandle;
    private readonly nint _fp_GameControllerGetStringForAxis;
    private readonly nint _fp_GameControllerGetStringForButton;
    private readonly nint _fp_GameControllerGetTouchpadFinger;
    private readonly nint _fp_GameControllerGetType;
    private readonly nint _fp_GameControllerGetVendor;
    private readonly nint _fp_GameControllerHasAxis;
    private readonly nint _fp_GameControllerHasButton;
    private readonly nint _fp_GameControllerHasLED;
    private readonly nint _fp_GameControllerHasRumble;
    private readonly nint _fp_GameControllerHasRumbleTriggers;
    private readonly nint _fp_GameControllerHasSensor;
    private readonly nint _fp_GameControllerIsSensorEnabled;
    private readonly nint _fp_GameControllerMapping;
    private readonly nint _fp_GameControllerMappingForDeviceIndex;
    private readonly nint _fp_GameControllerMappingForGUID;
    private readonly nint _fp_GameControllerMappingForIndex;
    private readonly nint _fp_GameControllerName;
    private readonly nint _fp_GameControllerNumMappings;
    private readonly nint _fp_GameControllerOpen;
    private readonly nint _fp_GameControllerPath;
    private readonly nint _fp_GameControllerPathForIndex;
    private readonly nint _fp_GameControllerRumble;
    private readonly nint _fp_GameControllerRumbleTriggers;
    private readonly nint _fp_GameControllerSendEffect;
    private readonly nint _fp_GameControllerSetLED;
    private readonly nint _fp_GameControllerSetPlayerIndex;
    private readonly nint _fp_GameControllerSetSensorEnabled;
    private readonly nint _fp_GameControllerTypeForIndex;
    private readonly nint _fp_GameControllerUpdate;
    private readonly nint _fp_GetAudioDeviceName;
    private readonly nint _fp_GetAudioDeviceStatus;
    private readonly nint _fp_GetCurrentDisplayMode;
    private readonly nint _fp_GetCurrentVideoDriver;
    private readonly nint _fp_GetDesktopDisplayMode;
    private readonly nint _fp_GetDisplayBounds;
    private readonly nint _fp_GetDisplayUsableBounds;
    private readonly nint _fp_GetError;
    private readonly nint _fp_GetHint;
    private readonly nint _fp_GetKeyName;
    private readonly nint _fp_GetNumAudioDevices;
    private readonly nint _fp_GetNumVideoDisplays;
    private readonly nint _fp_GetNumVideoDrivers;
    private readonly nint _fp_GetPerformanceCounter;
    private readonly nint _fp_GetPerformanceFrequency;
    private readonly nint _fp_GetRendererOutputSize;
    private readonly nint _fp_GetTicks;
    private readonly nint _fp_GetVersion;
    private readonly nint _fp_GetVideoDriver;
    private readonly nint _fp_GetWindowDisplayIndex;
    private readonly nint _fp_GetWindowFlags;
    private readonly nint _fp_GetWindowFromID;
    private readonly nint _fp_GetWindowID;
    private readonly nint _fp_GetWindowSize;
    private readonly nint _fp_GetWindowSurface;
    private readonly nint _fp_GlCreateContext;
    private readonly nint _fp_GlDeleteContext;
    private readonly nint _fp_GlGetProcAddress;
    private readonly nint _fp_GlLoadLibrary;
    private readonly nint _fp_GlMakeCurrent;
    private readonly nint _fp_GlSetAttribute;
    private readonly nint _fp_GlSetSwapInterval;
    private readonly nint _fp_GlSwapWindow;
    private readonly nint _fp_GlUnloadLibrary;
    private readonly nint _fp_Init;
    private readonly nint _fp_IsGameController;
    private readonly nint _fp_JoystickAttachVirtual;
    private readonly nint _fp_JoystickClose;
    private readonly nint _fp_JoystickDetachVirtual;
    private readonly nint _fp_JoystickEventState;
    private readonly nint _fp_JoystickFromInstanceID;
    private readonly nint _fp_JoystickFromPlayerIndex;
    private readonly nint _fp_JoystickGetAttached;
    private readonly nint _fp_JoystickGetAxis;
    private readonly nint _fp_JoystickGetAxisInitialState;
    private readonly nint _fp_JoystickGetBall;
    private readonly nint _fp_JoystickGetButton;
    private readonly nint _fp_JoystickGetDeviceGUID;
    private readonly nint _fp_JoystickGetDeviceInstanceID;
    private readonly nint _fp_JoystickGetDevicePlayerIndex;
    private readonly nint _fp_JoystickGetDeviceProduct;
    private readonly nint _fp_JoystickGetDeviceProductVersion;
    private readonly nint _fp_JoystickGetDeviceType;
    private readonly nint _fp_JoystickGetDeviceVendor;
    private readonly nint _fp_JoystickGetFirmwareVersion;
    private readonly nint _fp_JoystickGetGUID;
    private readonly nint _fp_JoystickGetGUIDFromString;
    private readonly nint _fp_JoystickGetGUIDString;
    private readonly nint _fp_JoystickGetHat;
    private readonly nint _fp_JoystickGetPlayerIndex;
    private readonly nint _fp_JoystickGetProduct;
    private readonly nint _fp_JoystickGetProductVersion;
    private readonly nint _fp_JoystickGetSerial;
    private readonly nint _fp_JoystickGetType;
    private readonly nint _fp_JoystickGetVendor;
    private readonly nint _fp_JoystickHasLED;
    private readonly nint _fp_JoystickHasRumble;
    private readonly nint _fp_JoystickHasRumbleTriggers;
    private readonly nint _fp_JoystickInstanceID;
    private readonly nint _fp_JoystickIsHaptic;
    private readonly nint _fp_JoystickIsVirtual;
    private readonly nint _fp_JoystickName;
    private readonly nint _fp_JoystickNameForIndex;
    private readonly nint _fp_JoystickNumAxes;
    private readonly nint _fp_JoystickNumBalls;
    private readonly nint _fp_JoystickNumButtons;
    private readonly nint _fp_JoystickNumHats;
    private readonly nint _fp_JoystickOpen;
    private readonly nint _fp_JoystickPath;
    private readonly nint _fp_JoystickPathForIndex;
    private readonly nint _fp_JoystickRumble;
    private readonly nint _fp_JoystickRumbleTriggers;
    private readonly nint _fp_JoystickSendEffect;
    private readonly nint _fp_JoystickSetLED;
    private readonly nint _fp_JoystickSetPlayerIndex;
    private readonly nint _fp_JoystickSetVirtualAxis;
    private readonly nint _fp_JoystickSetVirtualButton;
    private readonly nint _fp_JoystickSetVirtualHat;
    private readonly nint _fp_JoystickUpdate;
    private readonly nint _fp_LockAudioDevice;
    private readonly nint _fp_MaximizeWindow;
    private readonly nint _fp_NewAudioStream;
    private readonly nint _fp_NumJoysticks;
    private readonly nint _fp_OpenAudioDevice;
    private readonly nint _fp_PauseAudioDevice;
    private readonly nint _fp_PollEvent;
    private readonly nint _fp_PushEvent;
    private readonly nint _fp_QueueAudio;
    private readonly nint _fp_Quit;
    private readonly nint _fp_RegisterEvents;
    private readonly nint _fp_RenderClear;
    private readonly nint _fp_RenderFillRect;
    private readonly nint _fp_RenderPresent;
    private readonly nint _fp_RestoreWindow;
    private readonly nint _fp_SetHint;
    private readonly nint _fp_SetRenderDrawColor;
    private readonly nint _fp_SetWindowBordered;
    private readonly nint _fp_SetWindowFullscreen;
    private readonly nint _fp_SetWindowPosition;
    private readonly nint _fp_SetWindowResizable;
    private readonly nint _fp_SetWindowSize;
    private readonly nint _fp_SetWindowTitle;
    private readonly nint _fp_ShowCursor;
    private readonly nint _fp_StartTextInput;
    private readonly nint _fp_StopTextInput;
    private readonly nint _fp_UnlockAudioDevice;
    private readonly nint _fp_UpdateWindowSurface;
    private readonly nint _fp_WaitEvent;

    public Sdl2Library(
        System.Func<string, nint> loader)
    {
        _fp_AudioStreamAvailable = loader.Invoke(nameof(AudioStreamAvailable));
        _fp_AudioStreamFlush = loader.Invoke(nameof(AudioStreamFlush));
        _fp_AudioStreamGet = loader.Invoke(nameof(AudioStreamGet));
        _fp_AudioStreamPut = loader.Invoke(nameof(AudioStreamPut));
        _fp_BlitSurface = loader.Invoke(nameof(BlitSurface));
        _fp_ClearQueuedAudio = loader.Invoke(nameof(ClearQueuedAudio));
        _fp_CloseAudioDevice = loader.Invoke(nameof(CloseAudioDevice));
        _fp_CreateRenderer = loader.Invoke(nameof(CreateRenderer));
        _fp_CreateRGBSurface = loader.Invoke(nameof(CreateRGBSurface));
        _fp_CreateRGBSurfaceFrom = loader.Invoke(nameof(CreateRGBSurfaceFrom));
        _fp_CreateSoftwareRenderer = loader.Invoke(nameof(CreateSoftwareRenderer));
        _fp_CreateWindow = loader.Invoke(nameof(CreateWindow));
        _fp_DestroyRenderer = loader.Invoke(nameof(DestroyRenderer));
        _fp_DestroyWindow = loader.Invoke(nameof(DestroyWindow));
        _fp_FillRect = loader.Invoke(nameof(FillRect));
        _fp_Free = loader.Invoke(nameof(Free));
        _fp_FreeAudioStream = loader.Invoke(nameof(FreeAudioStream));
        _fp_FreeSurface = loader.Invoke(nameof(FreeSurface));
        _fp_GameControllerAddMapping = loader.Invoke(nameof(GameControllerAddMapping));
        _fp_GameControllerClose = loader.Invoke(nameof(GameControllerClose));
        _fp_GameControllerEventState = loader.Invoke(nameof(GameControllerEventState));
        _fp_GameControllerFromInstanceID = loader.Invoke(nameof(GameControllerFromInstanceID));
        _fp_GameControllerFromPlayerIndex = loader.Invoke(nameof(GameControllerFromPlayerIndex));
        _fp_GameControllerGetAttached = loader.Invoke(nameof(GameControllerGetAttached));
        _fp_GameControllerGetAxis = loader.Invoke(nameof(GameControllerGetAxis));
        _fp_GameControllerGetAxisFromString = loader.Invoke(nameof(GameControllerGetAxisFromString));
        _fp_GameControllerGetBindForAxis = loader.Invoke(nameof(GameControllerGetBindForAxis));
        _fp_GameControllerGetBindForButton = loader.Invoke(nameof(GameControllerGetBindForButton));
        _fp_GameControllerGetButton = loader.Invoke(nameof(GameControllerGetButton));
        _fp_GameControllerGetButtonFromString = loader.Invoke(nameof(GameControllerGetButtonFromString));
        _fp_GameControllerGetFirmwareVersion = loader.Invoke(nameof(GameControllerGetFirmwareVersion));
        _fp_GameControllerGetJoystick = loader.Invoke(nameof(GameControllerGetJoystick));
        _fp_GameControllerGetNumTouchpadFingers = loader.Invoke(nameof(GameControllerGetNumTouchpadFingers));
        _fp_GameControllerGetNumTouchpads = loader.Invoke(nameof(GameControllerGetNumTouchpads));
        _fp_GameControllerGetPlayerIndex = loader.Invoke(nameof(GameControllerGetPlayerIndex));
        _fp_GameControllerGetProduct = loader.Invoke(nameof(GameControllerGetProduct));
        _fp_GameControllerGetProductVersion = loader.Invoke(nameof(GameControllerGetProductVersion));
        _fp_GameControllerGetSensorData = loader.Invoke(nameof(GameControllerGetSensorData));
        _fp_GameControllerGetSensorDataRate = loader.Invoke(nameof(GameControllerGetSensorDataRate));
        _fp_GameControllerGetSensorDataWithTimestamp = loader.Invoke(nameof(GameControllerGetSensorDataWithTimestamp));
        _fp_GameControllerGetSerial = loader.Invoke(nameof(GameControllerGetSerial));
        _fp_GameControllerGetSteamHandle = loader.Invoke(nameof(GameControllerGetSteamHandle));
        _fp_GameControllerGetStringForAxis = loader.Invoke(nameof(GameControllerGetStringForAxis));
        _fp_GameControllerGetStringForButton = loader.Invoke(nameof(GameControllerGetStringForButton));
        _fp_GameControllerGetTouchpadFinger = loader.Invoke(nameof(GameControllerGetTouchpadFinger));
        _fp_GameControllerGetType = loader.Invoke(nameof(GameControllerGetType));
        _fp_GameControllerGetVendor = loader.Invoke(nameof(GameControllerGetVendor));
        _fp_GameControllerHasAxis = loader.Invoke(nameof(GameControllerHasAxis));
        _fp_GameControllerHasButton = loader.Invoke(nameof(GameControllerHasButton));
        _fp_GameControllerHasLED = loader.Invoke(nameof(GameControllerHasLED));
        _fp_GameControllerHasRumble = loader.Invoke(nameof(GameControllerHasRumble));
        _fp_GameControllerHasRumbleTriggers = loader.Invoke(nameof(GameControllerHasRumbleTriggers));
        _fp_GameControllerHasSensor = loader.Invoke(nameof(GameControllerHasSensor));
        _fp_GameControllerIsSensorEnabled = loader.Invoke(nameof(GameControllerIsSensorEnabled));
        _fp_GameControllerMapping = loader.Invoke(nameof(GameControllerMapping));
        _fp_GameControllerMappingForDeviceIndex = loader.Invoke(nameof(GameControllerMappingForDeviceIndex));
        _fp_GameControllerMappingForGUID = loader.Invoke(nameof(GameControllerMappingForGUID));
        _fp_GameControllerMappingForIndex = loader.Invoke(nameof(GameControllerMappingForIndex));
        _fp_GameControllerName = loader.Invoke(nameof(GameControllerName));
        _fp_GameControllerNumMappings = loader.Invoke(nameof(GameControllerNumMappings));
        _fp_GameControllerOpen = loader.Invoke(nameof(GameControllerOpen));
        _fp_GameControllerPath = loader.Invoke(nameof(GameControllerPath));
        _fp_GameControllerPathForIndex = loader.Invoke(nameof(GameControllerPathForIndex));
        _fp_GameControllerRumble = loader.Invoke(nameof(GameControllerRumble));
        _fp_GameControllerRumbleTriggers = loader.Invoke(nameof(GameControllerRumbleTriggers));
        _fp_GameControllerSendEffect = loader.Invoke(nameof(GameControllerSendEffect));
        _fp_GameControllerSetLED = loader.Invoke(nameof(GameControllerSetLED));
        _fp_GameControllerSetPlayerIndex = loader.Invoke(nameof(GameControllerSetPlayerIndex));
        _fp_GameControllerSetSensorEnabled = loader.Invoke(nameof(GameControllerSetSensorEnabled));
        _fp_GameControllerTypeForIndex = loader.Invoke(nameof(GameControllerTypeForIndex));
        _fp_GameControllerUpdate = loader.Invoke(nameof(GameControllerUpdate));
        _fp_GetAudioDeviceName = loader.Invoke(nameof(GetAudioDeviceName));
        _fp_GetAudioDeviceStatus = loader.Invoke(nameof(GetAudioDeviceStatus));
        _fp_GetCurrentDisplayMode = loader.Invoke(nameof(GetCurrentDisplayMode));
        _fp_GetCurrentVideoDriver = loader.Invoke(nameof(GetCurrentVideoDriver));
        _fp_GetDesktopDisplayMode = loader.Invoke(nameof(GetDesktopDisplayMode));
        _fp_GetDisplayBounds = loader.Invoke(nameof(GetDisplayBounds));
        _fp_GetDisplayUsableBounds = loader.Invoke(nameof(GetDisplayUsableBounds));
        _fp_GetError = loader.Invoke(nameof(GetError));
        _fp_GetHint = loader.Invoke(nameof(GetHint));
        _fp_GetKeyName = loader.Invoke(nameof(GetKeyName));
        _fp_GetNumAudioDevices = loader.Invoke(nameof(GetNumAudioDevices));
        _fp_GetNumVideoDisplays = loader.Invoke(nameof(GetNumVideoDisplays));
        _fp_GetNumVideoDrivers = loader.Invoke(nameof(GetNumVideoDrivers));
        _fp_GetPerformanceCounter = loader.Invoke(nameof(GetPerformanceCounter));
        _fp_GetPerformanceFrequency = loader.Invoke(nameof(GetPerformanceFrequency));
        _fp_GetRendererOutputSize = loader.Invoke(nameof(GetRendererOutputSize));
        _fp_GetTicks = loader.Invoke(nameof(GetTicks));
        _fp_GetVersion = loader.Invoke(nameof(GetVersion));
        _fp_GetVideoDriver = loader.Invoke(nameof(GetVideoDriver));
        _fp_GetWindowDisplayIndex = loader.Invoke(nameof(GetWindowDisplayIndex));
        _fp_GetWindowFlags = loader.Invoke(nameof(GetWindowFlags));
        _fp_GetWindowFromID = loader.Invoke(nameof(GetWindowFromID));
        _fp_GetWindowID = loader.Invoke(nameof(GetWindowID));
        _fp_GetWindowSize = loader.Invoke(nameof(GetWindowSize));
        _fp_GetWindowSurface = loader.Invoke(nameof(GetWindowSurface));
        _fp_GlCreateContext = loader.Invoke(nameof(GlCreateContext));
        _fp_GlDeleteContext = loader.Invoke(nameof(GlDeleteContext));
        _fp_GlGetProcAddress = loader.Invoke(nameof(GlGetProcAddress));
        _fp_GlLoadLibrary = loader.Invoke(nameof(GlLoadLibrary));
        _fp_GlMakeCurrent = loader.Invoke(nameof(GlMakeCurrent));
        _fp_GlSetAttribute = loader.Invoke(nameof(GlSetAttribute));
        _fp_GlSetSwapInterval = loader.Invoke(nameof(GlSetSwapInterval));
        _fp_GlSwapWindow = loader.Invoke(nameof(GlSwapWindow));
        _fp_GlUnloadLibrary = loader.Invoke(nameof(GlUnloadLibrary));
        _fp_Init = loader.Invoke(nameof(Init));
        _fp_IsGameController = loader.Invoke(nameof(IsGameController));
        _fp_JoystickAttachVirtual = loader.Invoke(nameof(JoystickAttachVirtual));
        _fp_JoystickClose = loader.Invoke(nameof(JoystickClose));
        _fp_JoystickDetachVirtual = loader.Invoke(nameof(JoystickDetachVirtual));
        _fp_JoystickEventState = loader.Invoke(nameof(JoystickEventState));
        _fp_JoystickFromInstanceID = loader.Invoke(nameof(JoystickFromInstanceID));
        _fp_JoystickFromPlayerIndex = loader.Invoke(nameof(JoystickFromPlayerIndex));
        _fp_JoystickGetAttached = loader.Invoke(nameof(JoystickGetAttached));
        _fp_JoystickGetAxis = loader.Invoke(nameof(JoystickGetAxis));
        _fp_JoystickGetAxisInitialState = loader.Invoke(nameof(JoystickGetAxisInitialState));
        _fp_JoystickGetBall = loader.Invoke(nameof(JoystickGetBall));
        _fp_JoystickGetButton = loader.Invoke(nameof(JoystickGetButton));
        _fp_JoystickGetDeviceGUID = loader.Invoke(nameof(JoystickGetDeviceGUID));
        _fp_JoystickGetDeviceInstanceID = loader.Invoke(nameof(JoystickGetDeviceInstanceID));
        _fp_JoystickGetDevicePlayerIndex = loader.Invoke(nameof(JoystickGetDevicePlayerIndex));
        _fp_JoystickGetDeviceProduct = loader.Invoke(nameof(JoystickGetDeviceProduct));
        _fp_JoystickGetDeviceProductVersion = loader.Invoke(nameof(JoystickGetDeviceProductVersion));
        _fp_JoystickGetDeviceType = loader.Invoke(nameof(JoystickGetDeviceType));
        _fp_JoystickGetDeviceVendor = loader.Invoke(nameof(JoystickGetDeviceVendor));
        _fp_JoystickGetFirmwareVersion = loader.Invoke(nameof(JoystickGetFirmwareVersion));
        _fp_JoystickGetGUID = loader.Invoke(nameof(JoystickGetGUID));
        _fp_JoystickGetGUIDFromString = loader.Invoke(nameof(JoystickGetGUIDFromString));
        _fp_JoystickGetGUIDString = loader.Invoke(nameof(JoystickGetGUIDString));
        _fp_JoystickGetHat = loader.Invoke(nameof(JoystickGetHat));
        _fp_JoystickGetPlayerIndex = loader.Invoke(nameof(JoystickGetPlayerIndex));
        _fp_JoystickGetProduct = loader.Invoke(nameof(JoystickGetProduct));
        _fp_JoystickGetProductVersion = loader.Invoke(nameof(JoystickGetProductVersion));
        _fp_JoystickGetSerial = loader.Invoke(nameof(JoystickGetSerial));
        _fp_JoystickGetType = loader.Invoke(nameof(JoystickGetType));
        _fp_JoystickGetVendor = loader.Invoke(nameof(JoystickGetVendor));
        _fp_JoystickHasLED = loader.Invoke(nameof(JoystickHasLED));
        _fp_JoystickHasRumble = loader.Invoke(nameof(JoystickHasRumble));
        _fp_JoystickHasRumbleTriggers = loader.Invoke(nameof(JoystickHasRumbleTriggers));
        _fp_JoystickInstanceID = loader.Invoke(nameof(JoystickInstanceID));
        _fp_JoystickIsHaptic = loader.Invoke(nameof(JoystickIsHaptic));
        _fp_JoystickIsVirtual = loader.Invoke(nameof(JoystickIsVirtual));
        _fp_JoystickName = loader.Invoke(nameof(JoystickName));
        _fp_JoystickNameForIndex = loader.Invoke(nameof(JoystickNameForIndex));
        _fp_JoystickNumAxes = loader.Invoke(nameof(JoystickNumAxes));
        _fp_JoystickNumBalls = loader.Invoke(nameof(JoystickNumBalls));
        _fp_JoystickNumButtons = loader.Invoke(nameof(JoystickNumButtons));
        _fp_JoystickNumHats = loader.Invoke(nameof(JoystickNumHats));
        _fp_JoystickOpen = loader.Invoke(nameof(JoystickOpen));
        _fp_JoystickPath = loader.Invoke(nameof(JoystickPath));
        _fp_JoystickPathForIndex = loader.Invoke(nameof(JoystickPathForIndex));
        _fp_JoystickRumble = loader.Invoke(nameof(JoystickRumble));
        _fp_JoystickRumbleTriggers = loader.Invoke(nameof(JoystickRumbleTriggers));
        _fp_JoystickSendEffect = loader.Invoke(nameof(JoystickSendEffect));
        _fp_JoystickSetLED = loader.Invoke(nameof(JoystickSetLED));
        _fp_JoystickSetPlayerIndex = loader.Invoke(nameof(JoystickSetPlayerIndex));
        _fp_JoystickSetVirtualAxis = loader.Invoke(nameof(JoystickSetVirtualAxis));
        _fp_JoystickSetVirtualButton = loader.Invoke(nameof(JoystickSetVirtualButton));
        _fp_JoystickSetVirtualHat = loader.Invoke(nameof(JoystickSetVirtualHat));
        _fp_JoystickUpdate = loader.Invoke(nameof(JoystickUpdate));
        _fp_LockAudioDevice = loader.Invoke(nameof(LockAudioDevice));
        _fp_MaximizeWindow = loader.Invoke(nameof(MaximizeWindow));
        _fp_NewAudioStream = loader.Invoke(nameof(NewAudioStream));
        _fp_NumJoysticks = loader.Invoke(nameof(NumJoysticks));
        _fp_OpenAudioDevice = loader.Invoke(nameof(OpenAudioDevice));
        _fp_PauseAudioDevice = loader.Invoke(nameof(PauseAudioDevice));
        _fp_PollEvent = loader.Invoke(nameof(PollEvent));
        _fp_PushEvent = loader.Invoke(nameof(PushEvent));
        _fp_QueueAudio = loader.Invoke(nameof(QueueAudio));
        _fp_Quit = loader.Invoke(nameof(Quit));
        _fp_RegisterEvents = loader.Invoke(nameof(RegisterEvents));
        _fp_RenderClear = loader.Invoke(nameof(RenderClear));
        _fp_RenderFillRect = loader.Invoke(nameof(RenderFillRect));
        _fp_RenderPresent = loader.Invoke(nameof(RenderPresent));
        _fp_RestoreWindow = loader.Invoke(nameof(RestoreWindow));
        _fp_SetHint = loader.Invoke(nameof(SetHint));
        _fp_SetRenderDrawColor = loader.Invoke(nameof(SetRenderDrawColor));
        _fp_SetWindowBordered = loader.Invoke(nameof(SetWindowBordered));
        _fp_SetWindowFullscreen = loader.Invoke(nameof(SetWindowFullscreen));
        _fp_SetWindowPosition = loader.Invoke(nameof(SetWindowPosition));
        _fp_SetWindowResizable = loader.Invoke(nameof(SetWindowResizable));
        _fp_SetWindowSize = loader.Invoke(nameof(SetWindowSize));
        _fp_SetWindowTitle = loader.Invoke(nameof(SetWindowTitle));
        _fp_ShowCursor = loader.Invoke(nameof(ShowCursor));
        _fp_StartTextInput = loader.Invoke(nameof(StartTextInput));
        _fp_StopTextInput = loader.Invoke(nameof(StopTextInput));
        _fp_UnlockAudioDevice = loader.Invoke(nameof(UnlockAudioDevice));
        _fp_UpdateWindowSurface = loader.Invoke(nameof(UpdateWindowSurface));
        _fp_WaitEvent = loader.Invoke(nameof(WaitEvent));
    }

    public int AudioStreamAvailable(
        nint stream)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_AudioStreamAvailable;
        var __result = __fp(stream);
        return __result;
    }

    public int AudioStreamFlush(
        nint stream)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_AudioStreamFlush;
        var __result = __fp(stream);
        return __result;
    }

    public int AudioStreamGet(
        nint stream,
        out float buffer,
        int length)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, int
            >)_fp_AudioStreamGet;
        fixed (void* __p_buffer = &buffer)
        {
            var __result = __fp(stream, __p_buffer, length);
            return __result;
        }
    }

    public int AudioStreamGet(
        nint stream,
        out byte buffer,
        int length)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, int
            >)_fp_AudioStreamGet;
        fixed (void* __p_buffer = &buffer)
        {
            var __result = __fp(stream, __p_buffer, length);
            return __result;
        }
    }

    public int AudioStreamGet(
        nint stream,
        out short buffer,
        int length)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, int
            >)_fp_AudioStreamGet;
        fixed (void* __p_buffer = &buffer)
        {
            var __result = __fp(stream, __p_buffer, length);
            return __result;
        }
    }

    public int AudioStreamPut(
        nint stream,
        in short buffer,
        int length)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, int
            >)_fp_AudioStreamPut;
        fixed (void* __p_buffer = &buffer)
        {
            var __result = __fp(stream, __p_buffer, length);
            return __result;
        }
    }

    public int AudioStreamPut(
        nint stream,
        in byte buffer,
        int length)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, int
            >)_fp_AudioStreamPut;
        fixed (void* __p_buffer = &buffer)
        {
            var __result = __fp(stream, __p_buffer, length);
            return __result;
        }
    }

    public int BlitSurface(
        nint source,
        in SdlRect sourceRectangle,
        nint destination,
        ref SdlRect destinationRectangle)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, nint, void*, int
            >)_fp_BlitSurface;
        fixed (void* __p_sourceRectangle = &sourceRectangle)
        fixed (void* __p_destinationRectangle = &destinationRectangle)
        {
            var __result = __fp(source, __p_sourceRectangle, destination, __p_destinationRectangle);
            return __result;
        }
    }

    public void ClearQueuedAudio(
        uint dev)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_ClearQueuedAudio;
        __fp(dev);
    }

    public void CloseAudioDevice(
        uint dev)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_CloseAudioDevice;
        __fp(dev);
    }

    public nint CreateRGBSurface(
        uint flags,
        int width,
        int height,
        int depth,
        uint rmask,
        uint gmask,
        uint bmask,
        uint amask)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, int, int, uint, uint, uint, uint, nint
            >)_fp_CreateRGBSurface;
        var __result = __fp(flags, width, height, depth, rmask, gmask, bmask, amask);
        return __result;
    }

    public nint CreateRGBSurfaceFrom(
        nint pixels,
        int width,
        int height,
        int depth,
        int pitch,
        uint rmask,
        uint gmask,
        uint bmask,
        uint amask)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int, int, int, uint, uint, uint, uint, nint
            >)_fp_CreateRGBSurfaceFrom;
        var __result = __fp(pixels, width, height, depth, pitch, rmask, gmask, bmask, amask);
        return __result;
    }

    public nint CreateRenderer(
        nint window,
        int index,
        SdlRendererFlags flags)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, SdlRendererFlags, nint
            >)_fp_CreateRenderer;
        var __result = __fp(window, index, flags);
        return __result;
    }

    public nint CreateSoftwareRenderer(
        nint surface)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, nint
            >)_fp_CreateSoftwareRenderer;
        var __result = __fp(surface);
        return __result;
    }

    public nint CreateWindow(
        string title,
        int x,
        int y,
        int w,
        int h,
        uint flags)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, int, int, int, int, uint, nint
            >)_fp_CreateWindow;
        var __result = __fp(title, x, y, w, h, flags);
        return __result;
    }

    public void DestroyRenderer(
        nint renderer)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_DestroyRenderer;
        __fp(renderer);
    }

    public void DestroyWindow(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_DestroyWindow;
        __fp(window);
    }

    public int FillRect(
        nint surface,
        ref readonly SdlRect rect,
        uint color)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, uint, int
            >)_fp_FillRect;
        fixed (void* __p_rect = &rect)
        {
            var __result = __fp(surface, __p_rect, color);
            return __result;
        }
    }

    public void Free(
        nint mem)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_Free;
        __fp(mem);
    }

    public void FreeAudioStream(
        nint stream)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_FreeAudioStream;
        __fp(stream);
    }

    public void FreeSurface(
        nint surface)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_FreeSurface;
        __fp(surface);
    }

    public int GameControllerAddMapping(
        string mappingString)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, int
            >)_fp_GameControllerAddMapping;
        var __result = __fp(mappingString);
        return __result;
    }

    public void GameControllerClose(
        nint gameController)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_GameControllerClose;
        __fp(gameController);
    }

    public int GameControllerEventState(
        int state)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int
            >)_fp_GameControllerEventState;
        var __result = __fp(state);
        return __result;
    }

    public nint GameControllerFromInstanceID(
        int joyid)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, nint
            >)_fp_GameControllerFromInstanceID;
        var __result = __fp(joyid);
        return __result;
    }

    public nint GameControllerFromPlayerIndex(
        int playerIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, nint
            >)_fp_GameControllerFromPlayerIndex;
        var __result = __fp(playerIndex);
        return __result;
    }

    public SdlBool GameControllerGetAttached(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlBool
            >)_fp_GameControllerGetAttached;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public short GameControllerGetAxis(
        nint gamecontroller,
        SdlControllerAxis axis)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlControllerAxis, short
            >)_fp_GameControllerGetAxis;
        var __result = __fp(gamecontroller, axis);
        return __result;
    }

    public SdlControllerAxis GameControllerGetAxisFromString(
        string str)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, SdlControllerAxis
            >)_fp_GameControllerGetAxisFromString;
        var __result = __fp(str);
        return __result;
    }

    public SdlGameControllerButtonBind GameControllerGetBindForAxis(
        nint gamecontroller,
        SdlControllerAxis axis)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlControllerAxis, SdlGameControllerButtonBind
            >)_fp_GameControllerGetBindForAxis;
        var __result = __fp(gamecontroller, axis);
        return __result;
    }

    public SdlGameControllerButtonBind GameControllerGetBindForButton(
        nint gamecontroller,
        SdlControllerButton button)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlControllerButton, SdlGameControllerButtonBind
            >)_fp_GameControllerGetBindForButton;
        var __result = __fp(gamecontroller, button);
        return __result;
    }

    public byte GameControllerGetButton(
        nint gamecontroller,
        SdlControllerButton button)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlControllerButton, byte
            >)_fp_GameControllerGetButton;
        var __result = __fp(gamecontroller, button);
        return __result;
    }

    public SdlControllerButton GameControllerGetButtonFromString(
        string str)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, SdlControllerButton
            >)_fp_GameControllerGetButtonFromString;
        var __result = __fp(str);
        return __result;
    }

    public ushort GameControllerGetFirmwareVersion(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort
            >)_fp_GameControllerGetFirmwareVersion;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public nint GameControllerGetJoystick(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, nint
            >)_fp_GameControllerGetJoystick;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public int GameControllerGetNumTouchpadFingers(
        nint gamecontroller,
        int touchpad)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int
            >)_fp_GameControllerGetNumTouchpadFingers;
        var __result = __fp(gamecontroller, touchpad);
        return __result;
    }

    public int GameControllerGetNumTouchpads(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_GameControllerGetNumTouchpads;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public int GameControllerGetPlayerIndex(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_GameControllerGetPlayerIndex;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public ushort GameControllerGetProduct(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort
            >)_fp_GameControllerGetProduct;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public ushort GameControllerGetProductVersion(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort
            >)_fp_GameControllerGetProductVersion;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public int GameControllerGetSensorData(
        nint gamecontroller,
        SdlSensorType type,
        out float data,
        int numValues)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlSensorType, void*, int, int
            >)_fp_GameControllerGetSensorData;
        fixed (void* __p_data = &data)
        {
            var __result = __fp(gamecontroller, type, __p_data, numValues);
            return __result;
        }
    }

    public float GameControllerGetSensorDataRate(
        nint gamecontroller,
        SdlSensorType type)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlSensorType, float
            >)_fp_GameControllerGetSensorDataRate;
        var __result = __fp(gamecontroller, type);
        return __result;
    }

    public int GameControllerGetSensorDataWithTimestamp(
        nint gamecontroller,
        SdlSensorType type,
        out ulong timestamp,
        out float data,
        int numValues)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlSensorType, void*, void*, int, int
            >)_fp_GameControllerGetSensorDataWithTimestamp;
        fixed (void* __p_timestamp = &timestamp)
        fixed (void* __p_data = &data)
        {
            var __result = __fp(gamecontroller, type, __p_timestamp, __p_data, numValues);
            return __result;
        }
    }

    public CString GameControllerGetSerial(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, CString
            >)_fp_GameControllerGetSerial;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public ulong GameControllerGetSteamHandle(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ulong
            >)_fp_GameControllerGetSteamHandle;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public CString GameControllerGetStringForAxis(
        SdlControllerAxis axis)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            SdlControllerAxis, CString
            >)_fp_GameControllerGetStringForAxis;
        var __result = __fp(axis);
        return __result;
    }

    public CString GameControllerGetStringForButton(
        SdlControllerButton button)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            SdlControllerButton, CString
            >)_fp_GameControllerGetStringForButton;
        var __result = __fp(button);
        return __result;
    }

    public int GameControllerGetTouchpadFinger(
        nint gamecontroller,
        int touchpad,
        int finger,
        out byte state,
        out float x,
        out float y,
        out float pressure)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int, void*, void*, void*, void*, int
            >)_fp_GameControllerGetTouchpadFinger;
        fixed (void* __p_state = &state)
        fixed (void* __p_x = &x)
        fixed (void* __p_y = &y)
        fixed (void* __p_pressure = &pressure)
        {
            var __result = __fp(gamecontroller, touchpad, finger, __p_state, __p_x, __p_y, __p_pressure);
            return __result;
        }
    }

    public SdlControllerType GameControllerGetType(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlControllerType
            >)_fp_GameControllerGetType;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public ushort GameControllerGetVendor(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort
            >)_fp_GameControllerGetVendor;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public SdlBool GameControllerHasAxis(
        nint gamecontroller,
        SdlControllerAxis axis)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlControllerAxis, SdlBool
            >)_fp_GameControllerHasAxis;
        var __result = __fp(gamecontroller, axis);
        return __result;
    }

    public SdlBool GameControllerHasButton(
        nint gamecontroller,
        SdlControllerButton button)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlControllerButton, SdlBool
            >)_fp_GameControllerHasButton;
        var __result = __fp(gamecontroller, button);
        return __result;
    }

    public SdlBool GameControllerHasLED(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlBool
            >)_fp_GameControllerHasLED;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public SdlBool GameControllerHasRumble(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlBool
            >)_fp_GameControllerHasRumble;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public SdlBool GameControllerHasRumbleTriggers(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlBool
            >)_fp_GameControllerHasRumbleTriggers;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public SdlBool GameControllerHasSensor(
        nint gamecontroller,
        SdlSensorType type)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlSensorType, SdlBool
            >)_fp_GameControllerHasSensor;
        var __result = __fp(gamecontroller, type);
        return __result;
    }

    public SdlBool GameControllerIsSensorEnabled(
        nint gamecontroller,
        SdlSensorType type)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlSensorType, SdlBool
            >)_fp_GameControllerIsSensorEnabled;
        var __result = __fp(gamecontroller, type);
        return __result;
    }

    public CString GameControllerMapping(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, CString
            >)_fp_GameControllerMapping;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public CString GameControllerMappingForDeviceIndex(
        int joystickIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, CString
            >)_fp_GameControllerMappingForDeviceIndex;
        var __result = __fp(joystickIndex);
        return __result;
    }

    public CString GameControllerMappingForGUID(
        System.Guid guid)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            System.Guid, CString
            >)_fp_GameControllerMappingForGUID;
        var __result = __fp(guid);
        return __result;
    }

    public CString GameControllerMappingForIndex(
        int mappingIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, CString
            >)_fp_GameControllerMappingForIndex;
        var __result = __fp(mappingIndex);
        return __result;
    }

    public CString GameControllerName(
        nint gameController)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, CString
            >)_fp_GameControllerName;
        var __result = __fp(gameController);
        return __result;
    }

    public int GameControllerNumMappings()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int
            >)_fp_GameControllerNumMappings;
        var __result = __fp();
        return __result;
    }

    public nint GameControllerOpen(
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, nint
            >)_fp_GameControllerOpen;
        var __result = __fp(index);
        return __result;
    }

    public CString GameControllerPath(
        nint gamecontroller)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, CString
            >)_fp_GameControllerPath;
        var __result = __fp(gamecontroller);
        return __result;
    }

    public CString GameControllerPathForIndex(
        int joystickIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, CString
            >)_fp_GameControllerPathForIndex;
        var __result = __fp(joystickIndex);
        return __result;
    }

    public int GameControllerRumble(
        nint gameController,
        ushort lowFrequencyRumble,
        ushort highFrequencyRumble,
        uint durationMs)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort, ushort, uint, int
            >)_fp_GameControllerRumble;
        var __result = __fp(gameController, lowFrequencyRumble, highFrequencyRumble, durationMs);
        return __result;
    }

    public int GameControllerRumbleTriggers(
        nint gamecontroller,
        ushort leftRumble,
        ushort rightRumble,
        uint durationMs)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort, ushort, uint, int
            >)_fp_GameControllerRumbleTriggers;
        var __result = __fp(gamecontroller, leftRumble, rightRumble, durationMs);
        return __result;
    }

    public int GameControllerSendEffect(
        nint gamecontroller,
        ref readonly byte data,
        int size)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, int
            >)_fp_GameControllerSendEffect;
        fixed (void* __p_data = &data)
        {
            var __result = __fp(gamecontroller, __p_data, size);
            return __result;
        }
    }

    public int GameControllerSetLED(
        nint gamecontroller,
        byte red,
        byte green,
        byte blue)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, byte, byte, byte, int
            >)_fp_GameControllerSetLED;
        var __result = __fp(gamecontroller, red, green, blue);
        return __result;
    }

    public void GameControllerSetPlayerIndex(
        nint gamecontroller,
        int playerIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, void
            >)_fp_GameControllerSetPlayerIndex;
        __fp(gamecontroller, playerIndex);
    }

    public int GameControllerSetSensorEnabled(
        nint gamecontroller,
        SdlSensorType type,
        SdlBool enabled)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlSensorType, SdlBool, int
            >)_fp_GameControllerSetSensorEnabled;
        var __result = __fp(gamecontroller, type, enabled);
        return __result;
    }

    public SdlControllerType GameControllerTypeForIndex(
        int joystickIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, SdlControllerType
            >)_fp_GameControllerTypeForIndex;
        var __result = __fp(joystickIndex);
        return __result;
    }

    public void GameControllerUpdate()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void
            >)_fp_GameControllerUpdate;
        __fp();
    }

    public CString GetAudioDeviceName(
        int index,
        int isCapture)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int, CString
            >)_fp_GetAudioDeviceName;
        var __result = __fp(index, isCapture);
        return __result;
    }

    public SdlAudioStatus GetAudioDeviceStatus(
        uint dev)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, SdlAudioStatus
            >)_fp_GetAudioDeviceStatus;
        var __result = __fp(dev);
        return __result;
    }

    public int GetCurrentDisplayMode(
        int displayIndex,
        out SdlDisplayMode mode)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, int
            >)_fp_GetCurrentDisplayMode;
        fixed (void* __p_mode = &mode)
        {
            var __result = __fp(displayIndex, __p_mode);
            return __result;
        }
    }

    public CString GetCurrentVideoDriver()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            CString
            >)_fp_GetCurrentVideoDriver;
        var __result = __fp();
        return __result;
    }

    public int GetDesktopDisplayMode(
        int displayIndex,
        out SdlDisplayMode mode)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, int
            >)_fp_GetDesktopDisplayMode;
        fixed (void* __p_mode = &mode)
        {
            var __result = __fp(displayIndex, __p_mode);
            return __result;
        }
    }

    public int GetDisplayBounds(
        int displayIndex,
        out SdlRect rect)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, int
            >)_fp_GetDisplayBounds;
        fixed (void* __p_rect = &rect)
        {
            var __result = __fp(displayIndex, __p_rect);
            return __result;
        }
    }

    public int GetDisplayUsableBounds(
        int displayIndex,
        out SdlRect rect)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, void*, int
            >)_fp_GetDisplayUsableBounds;
        fixed (void* __p_rect = &rect)
        {
            var __result = __fp(displayIndex, __p_rect);
            return __result;
        }
    }

    public CString GetError()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            CString
            >)_fp_GetError;
        var __result = __fp();
        return __result;
    }

    public CString GetHint(
        string name)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, CString
            >)_fp_GetHint;
        var __result = __fp(name);
        return __result;
    }

    public CString GetKeyName(
        int scanCode)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, CString
            >)_fp_GetKeyName;
        var __result = __fp(scanCode);
        return __result;
    }

    public int GetNumAudioDevices(
        int isCapture)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int
            >)_fp_GetNumAudioDevices;
        var __result = __fp(isCapture);
        return __result;
    }

    public int GetNumVideoDisplays()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int
            >)_fp_GetNumVideoDisplays;
        var __result = __fp();
        return __result;
    }

    public int GetNumVideoDrivers()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int
            >)_fp_GetNumVideoDrivers;
        var __result = __fp();
        return __result;
    }

    public ulong GetPerformanceCounter()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            ulong
            >)_fp_GetPerformanceCounter;
        var __result = __fp();
        return __result;
    }

    public ulong GetPerformanceFrequency()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            ulong
            >)_fp_GetPerformanceFrequency;
        var __result = __fp();
        return __result;
    }

    public int GetRendererOutputSize(
        nint renderer,
        out int w,
        out int h)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, void*, int
            >)_fp_GetRendererOutputSize;
        fixed (void* __p_w = &w)
        fixed (void* __p_h = &h)
        {
            var __result = __fp(renderer, __p_w, __p_h);
            return __result;
        }
    }

    public uint GetTicks()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint
            >)_fp_GetTicks;
        var __result = __fp();
        return __result;
    }

    public void GetVersion(
        out byte v)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, void
            >)_fp_GetVersion;
        fixed (void* __p_v = &v)
        {
            __fp(__p_v);
        }
    }

    public CString GetVideoDriver(
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, CString
            >)_fp_GetVideoDriver;
        var __result = __fp(index);
        return __result;
    }

    public int GetWindowDisplayIndex(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_GetWindowDisplayIndex;
        var __result = __fp(window);
        return __result;
    }

    public uint GetWindowFlags(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, uint
            >)_fp_GetWindowFlags;
        var __result = __fp(window);
        return __result;
    }

    public nint GetWindowFromID(
        uint windowId)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, nint
            >)_fp_GetWindowFromID;
        var __result = __fp(windowId);
        return __result;
    }

    public uint GetWindowID(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, uint
            >)_fp_GetWindowID;
        var __result = __fp(window);
        return __result;
    }

    public void GetWindowSize(
        nint window,
        out int width,
        out int height)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, void*, void
            >)_fp_GetWindowSize;
        fixed (void* __p_width = &width)
        fixed (void* __p_height = &height)
        {
            __fp(window, __p_width, __p_height);
        }
    }

    public nint GetWindowSurface(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, nint
            >)_fp_GetWindowSurface;
        var __result = __fp(window);
        return __result;
    }

    public nint GlCreateContext(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, nint
            >)_fp_GlCreateContext;
        var __result = __fp(window);
        return __result;
    }

    public void GlDeleteContext(
        nint context)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_GlDeleteContext;
        __fp(context);
    }

    public nint GlGetProcAddress(
        string proc)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, nint
            >)_fp_GlGetProcAddress;
        var __result = __fp(proc);
        return __result;
    }

    public int GlLoadLibrary(
        string? path)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string?, int
            >)_fp_GlLoadLibrary;
        var __result = __fp(path);
        return __result;
    }

    public int GlMakeCurrent(
        nint window,
        nint context)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, nint, int
            >)_fp_GlMakeCurrent;
        var __result = __fp(window, context);
        return __result;
    }

    public int GlSetAttribute(
        int attribute,
        int value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int, int
            >)_fp_GlSetAttribute;
        var __result = __fp(attribute, value);
        return __result;
    }

    public int GlSetSwapInterval(
        int interval)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int
            >)_fp_GlSetSwapInterval;
        var __result = __fp(interval);
        return __result;
    }

    public void GlSwapWindow(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_GlSwapWindow;
        __fp(window);
    }

    public void GlUnloadLibrary()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void
            >)_fp_GlUnloadLibrary;
        __fp();
    }

    public int Init(
        SdlInit flags)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            SdlInit, int
            >)_fp_Init;
        var __result = __fp(flags);
        return __result;
    }

    public int IsGameController(
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int
            >)_fp_IsGameController;
        var __result = __fp(index);
        return __result;
    }

    public int JoystickAttachVirtual(
        SdlJoystickType type,
        int naxes,
        int nbuttons,
        int nhats)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            SdlJoystickType, int, int, int, int
            >)_fp_JoystickAttachVirtual;
        var __result = __fp(type, naxes, nbuttons, nhats);
        return __result;
    }

    public void JoystickClose(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_JoystickClose;
        __fp(joystick);
    }

    public int JoystickDetachVirtual(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int
            >)_fp_JoystickDetachVirtual;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public int JoystickEventState(
        int state)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int
            >)_fp_JoystickEventState;
        var __result = __fp(state);
        return __result;
    }

    public nint JoystickFromInstanceID(
        uint instanceId)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, nint
            >)_fp_JoystickFromInstanceID;
        var __result = __fp(instanceId);
        return __result;
    }

    public nint JoystickFromPlayerIndex(
        int playerIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, nint
            >)_fp_JoystickFromPlayerIndex;
        var __result = __fp(playerIndex);
        return __result;
    }

    public SdlBool JoystickGetAttached(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlBool
            >)_fp_JoystickGetAttached;
        var __result = __fp(joystick);
        return __result;
    }

    public short JoystickGetAxis(
        nint joystick,
        int axis)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, short
            >)_fp_JoystickGetAxis;
        var __result = __fp(joystick, axis);
        return __result;
    }

    public SdlBool JoystickGetAxisInitialState(
        nint joystick,
        int axis,
        out short state)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, void*, SdlBool
            >)_fp_JoystickGetAxisInitialState;
        fixed (void* __p_state = &state)
        {
            var __result = __fp(joystick, axis, __p_state);
            return __result;
        }
    }

    public int JoystickGetBall(
        nint joystick,
        int ball,
        out int dx,
        out int dy)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, void*, void*, int
            >)_fp_JoystickGetBall;
        fixed (void* __p_dx = &dx)
        fixed (void* __p_dy = &dy)
        {
            var __result = __fp(joystick, ball, __p_dx, __p_dy);
            return __result;
        }
    }

    public byte JoystickGetButton(
        nint joystick,
        int button)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, byte
            >)_fp_JoystickGetButton;
        var __result = __fp(joystick, button);
        return __result;
    }

    public System.Guid JoystickGetDeviceGUID(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, System.Guid
            >)_fp_JoystickGetDeviceGUID;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public uint JoystickGetDeviceInstanceID(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, uint
            >)_fp_JoystickGetDeviceInstanceID;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public int JoystickGetDevicePlayerIndex(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int
            >)_fp_JoystickGetDevicePlayerIndex;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public ushort JoystickGetDeviceProduct(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, ushort
            >)_fp_JoystickGetDeviceProduct;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public ushort JoystickGetDeviceProductVersion(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, ushort
            >)_fp_JoystickGetDeviceProductVersion;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public SdlJoystickType JoystickGetDeviceType(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, SdlJoystickType
            >)_fp_JoystickGetDeviceType;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public ushort JoystickGetDeviceVendor(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, ushort
            >)_fp_JoystickGetDeviceVendor;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public ushort JoystickGetFirmwareVersion(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort
            >)_fp_JoystickGetFirmwareVersion;
        var __result = __fp(joystick);
        return __result;
    }

    public System.Guid JoystickGetGUID(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, System.Guid
            >)_fp_JoystickGetGUID;
        var __result = __fp(joystick);
        return __result;
    }

    public System.Guid JoystickGetGUIDFromString(
        string pchGuid)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, System.Guid
            >)_fp_JoystickGetGUIDFromString;
        var __result = __fp(pchGuid);
        return __result;
    }

    public void JoystickGetGUIDString(
        System.Guid guid,
        out byte guidBuffer,
        int byteCount)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            System.Guid, void*, int, void
            >)_fp_JoystickGetGUIDString;
        fixed (void* __p_guidBuffer = &guidBuffer)
        {
            __fp(guid, __p_guidBuffer, byteCount);
        }
    }

    public byte JoystickGetHat(
        nint joystick,
        int hat)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, byte
            >)_fp_JoystickGetHat;
        var __result = __fp(joystick, hat);
        return __result;
    }

    public int JoystickGetPlayerIndex(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_JoystickGetPlayerIndex;
        var __result = __fp(joystick);
        return __result;
    }

    public ushort JoystickGetProduct(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort
            >)_fp_JoystickGetProduct;
        var __result = __fp(joystick);
        return __result;
    }

    public ushort JoystickGetProductVersion(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort
            >)_fp_JoystickGetProductVersion;
        var __result = __fp(joystick);
        return __result;
    }

    public CString JoystickGetSerial(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, CString
            >)_fp_JoystickGetSerial;
        var __result = __fp(joystick);
        return __result;
    }

    public SdlJoystickType JoystickGetType(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlJoystickType
            >)_fp_JoystickGetType;
        var __result = __fp(joystick);
        return __result;
    }

    public ushort JoystickGetVendor(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort
            >)_fp_JoystickGetVendor;
        var __result = __fp(joystick);
        return __result;
    }

    public SdlBool JoystickHasLED(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlBool
            >)_fp_JoystickHasLED;
        var __result = __fp(joystick);
        return __result;
    }

    public SdlBool JoystickHasRumble(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlBool
            >)_fp_JoystickHasRumble;
        var __result = __fp(joystick);
        return __result;
    }

    public SdlBool JoystickHasRumbleTriggers(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, SdlBool
            >)_fp_JoystickHasRumbleTriggers;
        var __result = __fp(joystick);
        return __result;
    }

    public uint JoystickInstanceID(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, uint
            >)_fp_JoystickInstanceID;
        var __result = __fp(joystick);
        return __result;
    }

    public int JoystickIsHaptic(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_JoystickIsHaptic;
        var __result = __fp(joystick);
        return __result;
    }

    public SdlBool JoystickIsVirtual(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, SdlBool
            >)_fp_JoystickIsVirtual;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public CString JoystickName(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, CString
            >)_fp_JoystickName;
        var __result = __fp(joystick);
        return __result;
    }

    public CString JoystickNameForIndex(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, CString
            >)_fp_JoystickNameForIndex;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public int JoystickNumAxes(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_JoystickNumAxes;
        var __result = __fp(joystick);
        return __result;
    }

    public int JoystickNumBalls(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_JoystickNumBalls;
        var __result = __fp(joystick);
        return __result;
    }

    public int JoystickNumButtons(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_JoystickNumButtons;
        var __result = __fp(joystick);
        return __result;
    }

    public int JoystickNumHats(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_JoystickNumHats;
        var __result = __fp(joystick);
        return __result;
    }

    public nint JoystickOpen(
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, nint
            >)_fp_JoystickOpen;
        var __result = __fp(index);
        return __result;
    }

    public CString JoystickPath(
        nint joystick)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, CString
            >)_fp_JoystickPath;
        var __result = __fp(joystick);
        return __result;
    }

    public CString JoystickPathForIndex(
        int deviceIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, CString
            >)_fp_JoystickPathForIndex;
        var __result = __fp(deviceIndex);
        return __result;
    }

    public int JoystickRumble(
        nint joystick,
        ushort lowFrequencyRumble,
        ushort highFrequencyRumble,
        uint durationMs)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort, ushort, uint, int
            >)_fp_JoystickRumble;
        var __result = __fp(joystick, lowFrequencyRumble, highFrequencyRumble, durationMs);
        return __result;
    }

    public int JoystickRumbleTriggers(
        nint joystick,
        ushort leftRumble,
        ushort rightRumble,
        uint durationMs)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, ushort, ushort, uint, int
            >)_fp_JoystickRumbleTriggers;
        var __result = __fp(joystick, leftRumble, rightRumble, durationMs);
        return __result;
    }

    public int JoystickSendEffect(
        nint joystick,
        ref readonly byte data,
        int size)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, int
            >)_fp_JoystickSendEffect;
        fixed (void* __p_data = &data)
        {
            var __result = __fp(joystick, __p_data, size);
            return __result;
        }
    }

    public int JoystickSetLED(
        nint joystick,
        byte red,
        byte green,
        byte blue)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, byte, byte, byte, int
            >)_fp_JoystickSetLED;
        var __result = __fp(joystick, red, green, blue);
        return __result;
    }

    public void JoystickSetPlayerIndex(
        nint joystick,
        int playerIndex)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, void
            >)_fp_JoystickSetPlayerIndex;
        __fp(joystick, playerIndex);
    }

    public int JoystickSetVirtualAxis(
        nint joystick,
        int axis,
        short value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, short, int
            >)_fp_JoystickSetVirtualAxis;
        var __result = __fp(joystick, axis, value);
        return __result;
    }

    public int JoystickSetVirtualButton(
        nint joystick,
        int button,
        byte value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, byte, int
            >)_fp_JoystickSetVirtualButton;
        var __result = __fp(joystick, button, value);
        return __result;
    }

    public int JoystickSetVirtualHat(
        nint joystick,
        int hat,
        byte value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, byte, int
            >)_fp_JoystickSetVirtualHat;
        var __result = __fp(joystick, hat, value);
        return __result;
    }

    public void JoystickUpdate()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void
            >)_fp_JoystickUpdate;
        __fp();
    }

    public void LockAudioDevice(
        uint dev)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_LockAudioDevice;
        __fp(dev);
    }

    public void MaximizeWindow(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_MaximizeWindow;
        __fp(window);
    }

    public nint NewAudioStream(
        SdlAudioFormat sourceFormat,
        byte sourceChannels,
        int sourceRate,
        SdlAudioFormat destinationFormat,
        byte destinationChannels,
        int destinationRate)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            SdlAudioFormat, byte, int, SdlAudioFormat, byte, int, nint
            >)_fp_NewAudioStream;
        var __result = __fp(sourceFormat, sourceChannels, sourceRate, destinationFormat, destinationChannels, destinationRate);
        return __result;
    }

    public int NumJoysticks()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int
            >)_fp_NumJoysticks;
        var __result = __fp();
        return __result;
    }

    public uint OpenAudioDevice(
        string? device,
        int isCapture,
        in SdlAudioSpec desired,
        out SdlAudioSpec obtained,
        SdlAudioAllowChange allowedChanges)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string?, int, void*, void*, SdlAudioAllowChange, uint
            >)_fp_OpenAudioDevice;
        fixed (void* __p_desired = &desired)
        fixed (void* __p_obtained = &obtained)
        {
            var __result = __fp(device, isCapture, __p_desired, __p_obtained, allowedChanges);
            return __result;
        }
    }

    public void PauseAudioDevice(
        uint dev,
        int pauseOn)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, int, void
            >)_fp_PauseAudioDevice;
        __fp(dev, pauseOn);
    }

    public int PollEvent(
        out SdlEvent eventData)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, int
            >)_fp_PollEvent;
        fixed (void* __p_eventData = &eventData)
        {
            var __result = __fp(__p_eventData);
            return __result;
        }
    }

    public int PushEvent(
        in byte eventData)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, int
            >)_fp_PushEvent;
        fixed (void* __p_eventData = &eventData)
        {
            var __result = __fp(__p_eventData);
            return __result;
        }
    }

    public int QueueAudio(
        uint dev,
        ref readonly float data,
        uint len)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void*, uint, int
            >)_fp_QueueAudio;
        fixed (void* __p_data = &data)
        {
            var __result = __fp(dev, __p_data, len);
            return __result;
        }
    }

    public int QueueAudio(
        uint dev,
        ref readonly byte data,
        uint len)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void*, uint, int
            >)_fp_QueueAudio;
        fixed (void* __p_data = &data)
        {
            var __result = __fp(dev, __p_data, len);
            return __result;
        }
    }

    public void Quit()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void
            >)_fp_Quit;
        __fp();
    }

    public uint RegisterEvents(
        int numEvents)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, uint
            >)_fp_RegisterEvents;
        var __result = __fp(numEvents);
        return __result;
    }

    public int RenderClear(
        nint renderer)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_RenderClear;
        var __result = __fp(renderer);
        return __result;
    }

    public int RenderFillRect(
        nint renderer,
        ref readonly SdlRect rect)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int
            >)_fp_RenderFillRect;
        fixed (void* __p_rect = &rect)
        {
            var __result = __fp(renderer, __p_rect);
            return __result;
        }
    }

    public void RenderPresent(
        nint renderer)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_RenderPresent;
        __fp(renderer);
    }

    public void RestoreWindow(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_RestoreWindow;
        __fp(window);
    }

    public int SetHint(
        string name,
        string value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, string, int
            >)_fp_SetHint;
        var __result = __fp(name, value);
        return __result;
    }

    public int SetRenderDrawColor(
        nint renderer,
        byte r,
        byte g,
        byte b,
        byte a)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, byte, byte, byte, byte, int
            >)_fp_SetRenderDrawColor;
        var __result = __fp(renderer, r, g, b, a);
        return __result;
    }

    public void SetWindowBordered(
        nint window,
        int bordered)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, void
            >)_fp_SetWindowBordered;
        __fp(window, bordered);
    }

    public int SetWindowFullscreen(
        nint window,
        uint flags)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, uint, int
            >)_fp_SetWindowFullscreen;
        var __result = __fp(window, flags);
        return __result;
    }

    public void SetWindowPosition(
        nint window,
        int x,
        int y)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int, void
            >)_fp_SetWindowPosition;
        __fp(window, x, y);
    }

    public void SetWindowResizable(
        nint window,
        int resizable)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, void
            >)_fp_SetWindowResizable;
        __fp(window, resizable);
    }

    public void SetWindowSize(
        nint window,
        int width,
        int height)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int, void
            >)_fp_SetWindowSize;
        __fp(window, width, height);
    }

    public void SetWindowTitle(
        nint window,
        string title)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, string, void
            >)_fp_SetWindowTitle;
        __fp(window, title);
    }

    public int ShowCursor(
        int toggle)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int
            >)_fp_ShowCursor;
        var __result = __fp(toggle);
        return __result;
    }

    public void StartTextInput()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void
            >)_fp_StartTextInput;
        __fp();
    }

    public void StopTextInput()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void
            >)_fp_StopTextInput;
        __fp();
    }

    public void UnlockAudioDevice(
        uint dev)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            uint, void
            >)_fp_UnlockAudioDevice;
        __fp(dev);
    }

    public int UpdateWindowSurface(
        nint window)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_UpdateWindowSurface;
        var __result = __fp(window);
        return __result;
    }

    public int WaitEvent(
        out byte eventData)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            void*, int
            >)_fp_WaitEvent;
        fixed (void* __p_eventData = &eventData)
        {
            var __result = __fp(__p_eventData);
            return __result;
        }
    }
}
