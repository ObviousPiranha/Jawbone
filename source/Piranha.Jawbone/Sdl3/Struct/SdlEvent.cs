using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Explicit, Size = 128)]
public struct SdlEvent // SDL_Event
{
    [FieldOffset(0)]
    public SdlEventType Type; // Uint32 type
    [FieldOffset(0)]
    public SdlCommonEvent Common; // SDL_CommonEvent common
    [FieldOffset(0)]
    public SdlDisplayEvent Display; // SDL_DisplayEvent display
    [FieldOffset(0)]
    public SdlWindowEvent Window; // SDL_WindowEvent window
    [FieldOffset(0)]
    public SdlKeyboardDeviceEvent Kdevice; // SDL_KeyboardDeviceEvent kdevice
    [FieldOffset(0)]
    public SdlKeyboardEvent Key; // SDL_KeyboardEvent key
    [FieldOffset(0)]
    public SdlTextEditingEvent Edit; // SDL_TextEditingEvent edit
    [FieldOffset(0)]
    public SdlTextEditingCandidatesEvent EditCandidates; // SDL_TextEditingCandidatesEvent edit_candidates
    [FieldOffset(0)]
    public SdlTextInputEvent Text; // SDL_TextInputEvent text
    [FieldOffset(0)]
    public SdlMouseDeviceEvent Mdevice; // SDL_MouseDeviceEvent mdevice
    [FieldOffset(0)]
    public SdlMouseMotionEvent Motion; // SDL_MouseMotionEvent motion
    [FieldOffset(0)]
    public SdlMouseButtonEvent Button; // SDL_MouseButtonEvent button
    [FieldOffset(0)]
    public SdlMouseWheelEvent Wheel; // SDL_MouseWheelEvent wheel
    [FieldOffset(0)]
    public SdlJoyDeviceEvent Jdevice; // SDL_JoyDeviceEvent jdevice
    [FieldOffset(0)]
    public SdlJoyAxisEvent Jaxis; // SDL_JoyAxisEvent jaxis
    [FieldOffset(0)]
    public SdlJoyBallEvent Jball; // SDL_JoyBallEvent jball
    [FieldOffset(0)]
    public SdlJoyHatEvent Jhat; // SDL_JoyHatEvent jhat
    [FieldOffset(0)]
    public SdlJoyButtonEvent Jbutton; // SDL_JoyButtonEvent jbutton
    [FieldOffset(0)]
    public SdlJoyBatteryEvent Jbattery; // SDL_JoyBatteryEvent jbattery
    [FieldOffset(0)]
    public SdlGamepadDeviceEvent Gdevice; // SDL_GamepadDeviceEvent gdevice
    [FieldOffset(0)]
    public SdlGamepadAxisEvent Gaxis; // SDL_GamepadAxisEvent gaxis
    [FieldOffset(0)]
    public SdlGamepadButtonEvent Gbutton; // SDL_GamepadButtonEvent gbutton
    [FieldOffset(0)]
    public SdlGamepadTouchpadEvent Gtouchpad; // SDL_GamepadTouchpadEvent gtouchpad
    [FieldOffset(0)]
    public SdlGamepadSensorEvent Gsensor; // SDL_GamepadSensorEvent gsensor
    [FieldOffset(0)]
    public SdlAudioDeviceEvent Adevice; // SDL_AudioDeviceEvent adevice
    [FieldOffset(0)]
    public SdlCameraDeviceEvent Cdevice; // SDL_CameraDeviceEvent cdevice
    [FieldOffset(0)]
    public SdlSensorEvent Sensor; // SDL_SensorEvent sensor
    [FieldOffset(0)]
    public SdlQuitEvent Quit; // SDL_QuitEvent quit
    [FieldOffset(0)]
    public SdlUserEvent User; // SDL_UserEvent user
    [FieldOffset(0)]
    public SdlTouchFingerEvent Tfinger; // SDL_TouchFingerEvent tfinger
    [FieldOffset(0)]
    public SdlPenProximityEvent Pproximity; // SDL_PenProximityEvent pproximity
    [FieldOffset(0)]
    public SdlPenTouchEvent Ptouch; // SDL_PenTouchEvent ptouch
    [FieldOffset(0)]
    public SdlPenMotionEvent Pmotion; // SDL_PenMotionEvent pmotion
    [FieldOffset(0)]
    public SdlPenButtonEvent Pbutton; // SDL_PenButtonEvent pbutton
    [FieldOffset(0)]
    public SdlPenAxisEvent Paxis; // SDL_PenAxisEvent paxis
    [FieldOffset(0)]
    public SdlDropEvent Drop; // SDL_DropEvent drop
    [FieldOffset(0)]
    public SdlClipboardEvent Clipboard; // SDL_ClipboardEvent clipboard

    // Uint8[128] padding

    public static void Dispatch(in SdlEvent sdlEvent, ISdlEventHandler handler)
    {
        handler.OnCommonEvent(sdlEvent.Common);
        switch (sdlEvent.Type)
        {
            case SdlEventType.Quit:
                handler.OnQuit(sdlEvent.Quit);
                break;
            case SdlEventType.Terminating:
                handler.OnTerminating();
                break;
            case SdlEventType.LowMemory:
                handler.OnLowMemory();
                break;
            case SdlEventType.WillEnterBackground:
                handler.OnWillEnterBackground();
                break;
            case SdlEventType.DidEnterBackground:
                handler.OnDidEnterBackground();
                break;
            case SdlEventType.WillEnterForeground:
                handler.OnWillEnterForeground();
                break;
            case SdlEventType.DidEnterForeground:
                handler.OnDidEnterForeground();
                break;
            case SdlEventType.LocaleChanged:
                handler.OnLocaleChanged();
                break;
            case SdlEventType.DisplayOrientation:
                handler.OnDisplayOrientation(sdlEvent.Display);
                break;
            case SdlEventType.DisplayAdded:
                handler.OnDisplayAdded(sdlEvent.Display);
                break;
            case SdlEventType.DisplayRemoved:
                handler.OnDisplayRemoved(sdlEvent.Display);
                break;
            case SdlEventType.DisplayMoved:
                handler.OnDisplayMoved(sdlEvent.Display);
                break;
            case SdlEventType.DisplayContentScaleChanged:
                handler.OnDisplayContentScaleChanged(sdlEvent.Display);
                break;
            case SdlEventType.WindowHdrStateChanged:
                handler.OnWindowHdrStateChanged(sdlEvent.Display);
                break;
            case SdlEventType.WindowShown:
                handler.OnWindowShown(sdlEvent.Window);
                break;
            case SdlEventType.WindowHidden:
                handler.OnWindowHidden(sdlEvent.Window);
                break;
            case SdlEventType.WindowExposed:
                handler.OnWindowExposed(sdlEvent.Window);
                break;
            case SdlEventType.WindowMoved:
                handler.OnWindowMoved(sdlEvent.Window);
                break;
            case SdlEventType.WindowResized:
                handler.OnWindowResized(sdlEvent.Window);
                break;
            case SdlEventType.WindowPixelSizeChanged:
                handler.OnWindowPixelSizeChanged(sdlEvent.Window);
                break;
            case SdlEventType.WindowMinimized:
                handler.OnWindowMinimized(sdlEvent.Window);
                break;
            case SdlEventType.WindowMaximized:
                handler.OnWindowMaximized(sdlEvent.Window);
                break;
            case SdlEventType.WindowRestored:
                handler.OnWindowRestored(sdlEvent.Window);
                break;
            case SdlEventType.WindowMouseEnter:
                handler.OnWindowMouseEnter(sdlEvent.Window);
                break;
            case SdlEventType.WindowMouseLeave:
                handler.OnWindowMouseLeave(sdlEvent.Window);
                break;
            case SdlEventType.WindowFocusGained:
                handler.OnWindowFocusGained(sdlEvent.Window);
                break;
            case SdlEventType.WindowFocusLost:
                handler.OnWindowFocusLost(sdlEvent.Window);
                break;
            case SdlEventType.WindowCloseRequested:
                handler.OnWindowCloseRequested(sdlEvent.Window);
                break;
            case SdlEventType.WindowHitTest:
                handler.OnWindowHitTest(sdlEvent.Window);
                break;
            case SdlEventType.WindowIccprofChanged:
                handler.OnWindowIccProfChanged(sdlEvent.Window);
                break;
            case SdlEventType.WindowDisplayChanged:
                handler.OnWindowDisplayChanged(sdlEvent.Window);
                break;
            case SdlEventType.WindowDisplayScaleChanged:
                handler.OnWindowDisplayScaleChanged(sdlEvent.Window);
                break;
            case SdlEventType.WindowOccluded:
                handler.OnWindowOccluded(sdlEvent.Window);
                break;
            case SdlEventType.WindowEnterFullscreen:
                handler.OnWindowEnterFullscreen(sdlEvent.Window);
                break;
            case SdlEventType.WindowLeaveFullscreen:
                handler.OnWindowLeaveFullscreen(sdlEvent.Window);
                break;
            case SdlEventType.WindowDestroyed:
                handler.OnWindowDestroyed(sdlEvent.Window);
                break;
            case SdlEventType.KeyDown:
                handler.OnKeyDown(sdlEvent.Key);
                break;
            case SdlEventType.KeyUp:
                handler.OnKeyUp(sdlEvent.Key);
                break;
            case SdlEventType.TextEditing:
                handler.OnTextEditing(sdlEvent.Edit);
                break;
            case SdlEventType.TextInput:
                handler.OnTextInput(sdlEvent.Text);
                break;
            case SdlEventType.KeymapChanged:
                handler.OnKeymapChanged();
                break;
            case SdlEventType.KeyboardAdded:
                handler.OnKeyboardAdded(sdlEvent.Kdevice);
                break;
            case SdlEventType.KeyboardRemoved:
                handler.OnKeyboardRemoved(sdlEvent.Kdevice);
                break;
            case SdlEventType.MouseMotion:
                handler.OnMouseMotion(sdlEvent.Motion);
                break;
            case SdlEventType.MouseButtonDown:
                handler.OnMouseButtonDown(sdlEvent.Button);
                break;
            case SdlEventType.MouseButtonUp:
                handler.OnMouseButtonUp(sdlEvent.Button);
                break;
            case SdlEventType.MouseWheel:
                handler.OnMouseWheel(sdlEvent.Wheel);
                break;
            case SdlEventType.MouseAdded:
                handler.OnMouseAdded(sdlEvent.Mdevice);
                break;
            case SdlEventType.MouseRemoved:
                handler.OnMouseRemoved(sdlEvent.Mdevice);
                break;
            case SdlEventType.JoystickAxisMotion:
                handler.OnJoystickAxisMotion(sdlEvent.Jaxis);
                break;
            case SdlEventType.JoystickBallMotion:
                handler.OnJoystickBallMotion(sdlEvent.Jball);
                break;
            case SdlEventType.JoystickHatMotion:
                handler.OnJoystickHatMotion(sdlEvent.Jhat);
                break;
            case SdlEventType.JoystickButtonDown:
                handler.OnJoystickButtonDown(sdlEvent.Jbutton);
                break;
            case SdlEventType.JoystickButtonUp:
                handler.OnJoystickButtonUp(sdlEvent.Jbutton);
                break;
            case SdlEventType.JoystickAdded:
                handler.OnJoystickAdded(sdlEvent.Jdevice);
                break;
            case SdlEventType.JoystickRemoved:
                handler.OnJoystickRemoved(sdlEvent.Jdevice);
                break;
            case SdlEventType.JoystickBatteryUpdated:
                handler.OnJoystickBatteryUpdated(sdlEvent.Jbattery);
                break;
            case SdlEventType.JoystickUpdateComplete:
                handler.OnJoystickUpdateCompleted(sdlEvent.Jdevice);
                break;
            case SdlEventType.GamepadAxisMotion:
                handler.OnGamepadAxisMotion(sdlEvent.Gaxis);
                break;
            case SdlEventType.GamepadButtonDown:
                handler.OnGamepadButtonDown(sdlEvent.Gbutton);
                break;
            case SdlEventType.GamepadButtonUp:
                handler.OnGamepadButtonUp(sdlEvent.Gbutton);
                break;
            case SdlEventType.GamepadAdded:
                handler.OnGamepadDeviceAdded(sdlEvent.Gdevice);
                break;
            case SdlEventType.GamepadRemoved:
                handler.OnGamepadDeviceRemoved(sdlEvent.Gdevice);
                break;
            case SdlEventType.GamepadRemapped:
                handler.OnGamepadDeviceRemapped(sdlEvent.Gdevice);
                break;
            case SdlEventType.GamepadTouchpadDown:
                handler.OnGamepadTouchpadDown(sdlEvent.Gtouchpad);
                break;
            case SdlEventType.GamepadTouchpadMotion:
                handler.OnGamepadTouchpadMotion(sdlEvent.Gtouchpad);
                break;
            case SdlEventType.GamepadTouchpadUp:
                handler.OnGamepadTouchpadUp(sdlEvent.Gtouchpad);
                break;
            case SdlEventType.GamepadSensorUpdate:
                handler.OnGamepadSensorUpdate(sdlEvent.Gsensor);
                break;
            case SdlEventType.GamepadUpdateComplete:
                handler.OnGamepadUpdateComplete(sdlEvent.Gdevice);
                break;
            case SdlEventType.GamepadSteamHandleUpdated:
                handler.OnGamepadSteamHandleUpdated(sdlEvent.Gdevice);
                break;
            case SdlEventType.FingerDown:
                handler.OnFingerDown(sdlEvent.Tfinger);
                break;
            case SdlEventType.FingerUp:
                handler.OnFingerUp(sdlEvent.Tfinger);
                break;
            case SdlEventType.FingerMotion:
                handler.OnFingerMotion(sdlEvent.Tfinger);
                break;
            case SdlEventType.ClipboardUpdate:
                handler.OnClipboardUpdate(sdlEvent.Clipboard);
                break;
            case SdlEventType.DropFile:
                handler.OnDropFile(sdlEvent.Drop);
                break;
            case SdlEventType.DropText:
                handler.OnDropText(sdlEvent.Drop);
                break;
            case SdlEventType.DropBegin:
                handler.OnDropBegin(sdlEvent.Drop);
                break;
            case SdlEventType.DropComplete:
                handler.OnDropComplete(sdlEvent.Drop);
                break;
            case SdlEventType.DropPosition:
                handler.OnDropPosition(sdlEvent.Drop);
                break;
            case SdlEventType.AudioDeviceAdded:
                handler.OnAudioDeviceAdded(sdlEvent.Adevice);
                break;
            case SdlEventType.AudioDeviceRemoved:
                handler.OnAudioDeviceRemoved(sdlEvent.Adevice);
                break;
            case SdlEventType.AudioDeviceFormatChanged:
                handler.OnAudioDeviceFormatChanged(sdlEvent.Adevice);
                break;
            case SdlEventType.SensorUpdate:
                handler.OnSensorUpdate(sdlEvent.Sensor);
                break;
            case SdlEventType.PenDown:
                handler.OnPenDown(sdlEvent.Pproximity);
                break;
            case SdlEventType.PenUp:
                handler.OnPenUp(sdlEvent.Pproximity);
                break;
            case SdlEventType.PenMotion:
                handler.OnPenMotion(sdlEvent.Pmotion);
                break;
            case SdlEventType.PenButtonDown:
                handler.OnPenButtonDown(sdlEvent.Pbutton);
                break;
            case SdlEventType.PenButtonUp:
                handler.OnPenButtonUp(sdlEvent.Pbutton);
                break;
            case SdlEventType.CameraDeviceAdded:
                handler.OnCameraDeviceAdded(sdlEvent.Cdevice);
                break;
            case SdlEventType.CameraDeviceRemoved:
                handler.OnCameraDeviceRemoved(sdlEvent.Cdevice);
                break;
            case SdlEventType.CameraDeviceApproved:
                handler.OnCameraDeviceApproved(sdlEvent.Cdevice);
                break;
            case SdlEventType.CameraDeviceDenied:
                handler.OnCameraDeviceDenied(sdlEvent.Cdevice);
                break;
            case SdlEventType.RenderTargetsReset:
                handler.OnRenderTargetsReset();
                break;
            case SdlEventType.RenderDeviceReset:
                handler.OnRenderDeviceReset();
                break;
            default:
                if (SdlEventType.User <= sdlEvent.Type && sdlEvent.Type <= SdlEventType.Last)
                    handler.OnUserEvent(sdlEvent.User);
                break;
        }
    }
}
