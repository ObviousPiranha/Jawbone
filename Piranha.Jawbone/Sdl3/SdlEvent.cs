using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Explicit)]
public struct SdlEvent
{
    [FieldOffset(0)] public SdlEventType Type;
    [FieldOffset(0)] public SdlCommonEvent Common;
    [FieldOffset(0)] public SdlDisplayEvent Display;
    [FieldOffset(0)] public SdlWindowEvent Window;
    [FieldOffset(0)] public SdlKeyboardDeviceEvent KDevice;
    [FieldOffset(0)] public SdlKeyboardEvent Key;
    [FieldOffset(0)] public SdlTextEditingEvent Edit;
    [FieldOffset(0)] public SdlTextEditingExtEvent EditExt;
    [FieldOffset(0)] public SdlTextInputEvent Text;
    [FieldOffset(0)] public SdlMouseDeviceEvent MDevice;
    [FieldOffset(0)] public SdlMouseMotionEvent Motion;
    [FieldOffset(0)] public SdlMouseButtonEvent Button;
    [FieldOffset(0)] public SdlMouseWheelEvent Wheel;
    [FieldOffset(0)] public SdlJoyAxisEvent JAxis;
    [FieldOffset(0)] public SdlJoyBallEvent JBall;
    [FieldOffset(0)] public SdlJoyHatEvent JHat;
    [FieldOffset(0)] public SdlJoyButtonEvent JButton;
    [FieldOffset(0)] public SdlJoyDeviceEvent JDevice;
    [FieldOffset(0)] public SdlJoyBatteryEvent JBattery;
    [FieldOffset(0)] public SdlControllerAxisEvent GAxis;
    [FieldOffset(0)] public SdlControllerButtonEvent GButton;
    [FieldOffset(0)] public SdlGamepadDeviceEvent GDevice;
    [FieldOffset(0)] public SdlControllerTouchpadEvent GTouchpad;
    [FieldOffset(0)] public SdlGamepadSensorEvent GSensor;
    [FieldOffset(0)] public SdlControllerSensorEvent CSensor;
    [FieldOffset(0)] public SdlAudioDeviceEvent ADevice;
    [FieldOffset(0)] public SdlSensorEvent Sensor;
    [FieldOffset(0)] public SdlQuitEvent Quit;
    [FieldOffset(0)] public SdlUserEvent User;
    [FieldOffset(0)] public SdlTouchFingerEvent TFinger;
    [FieldOffset(0)] public SdlPenTipEvent PTip;
    [FieldOffset(0)] public SdlPenMotionEvent PMotion;
    [FieldOffset(0)] public SdlPenButtonEvent PButton;
    [FieldOffset(0)] public SdlDropEvent Drop;
    [FieldOffset(0)] public SdlClipboardEvent Clipboard;
    [FieldOffset(0)] private PaddingArray _padding;

    public static void Dispatch(Sdl3Library sdl, in SdlEvent sdlEvent, ISdlEventHandler handler)
    {
        switch (sdlEvent.Type)
        {
            case SdlEventType.Quit:
                handler.OnQuit();
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
            case SdlEventType.DisplayHdrStateChanged:
                handler.OnDisplayHdrStateChanged(sdlEvent.Display);
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
            case SdlEventType.WindowTakeFocus:
                handler.OnWindowTakeFocus(sdlEvent.Window);
                break;
            case SdlEventType.WindowHitTest:
                handler.OnWindowHitTest(sdlEvent.Window);
                break;
            case SdlEventType.WindowIccProfChanged:
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
            case SdlEventType.WindowPenEnter:
                handler.OnWindowPenEnter(sdlEvent.Window);
                break;
            case SdlEventType.WindowPenLeave:
                handler.OnWindowPenLeave(sdlEvent.Window);
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
            case SdlEventType.KeyMapChanged:
                handler.OnKeyMapChanged();
                break;
            case SdlEventType.KeyboardAdded:
                handler.OnKeyboardAdded(sdlEvent.KDevice);
                break;
            case SdlEventType.KeyboardRemoved:
                handler.OnKeyboardRemoved(sdlEvent.KDevice);
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
                handler.OnMouseAdded(sdlEvent.MDevice);
                break;
            case SdlEventType.MouseRemoved:
                handler.OnMouseRemoved(sdlEvent.MDevice);
                break;
            case SdlEventType.JoystickAxisMotion:
                handler.OnJoystickAxisMotion(sdlEvent.JAxis);
                break;
            case SdlEventType.JoystickBallMotion:
                handler.OnJoystickBallMotion(sdlEvent.JBall);
                break;
            case SdlEventType.JoystickHatMotion:
                handler.OnJoystickHatMotion(sdlEvent.JHat);
                break;
            case SdlEventType.JoystickButtonDown:
                handler.OnJoystickButtonDown(sdlEvent.JButton);
                break;
            case SdlEventType.JoystickButtonUp:
                handler.OnJoystickButtonUp(sdlEvent.JButton);
                break;
            case SdlEventType.JoystickAdded:
                handler.OnJoystickAdded(sdlEvent.JDevice);
                break;
            case SdlEventType.JoystickRemoved:
                handler.OnJoystickRemoved(sdlEvent.JDevice);
                break;
            case SdlEventType.JoystickBatteryUpdated:
                handler.OnJoystickBatteryUpdated(sdlEvent.JBattery);
                break;
            case SdlEventType.JoystickUpdateComplete:
                handler.OnJoystickUpdateCompleted(sdlEvent.JDevice);
                break;
            case SdlEventType.GamepadAxisMotion:
                handler.OnGamepadAxisMotion(sdlEvent.GAxis);
                break;
            case SdlEventType.GamepadButtonDown:
                handler.OnGamepadButtonDown(sdlEvent.GButton);
                break;
            case SdlEventType.GamepadButtonUp:
                handler.OnGamepadButtonUp(sdlEvent.GButton);
                break;
            case SdlEventType.GamepadAdded:
                handler.OnGamepadDeviceAdded(sdlEvent.GDevice);
                break;
            case SdlEventType.GamepadRemoved:
                handler.OnGamepadDeviceRemoved(sdlEvent.GDevice);
                break;
            case SdlEventType.GamepadRemapped:
                handler.OnGamepadDeviceRemapped(sdlEvent.GDevice);
                break;
            case SdlEventType.GamepadTouchpadDown:
                handler.OnGamepadTouchpadDown(sdlEvent.GTouchpad);
                break;
            case SdlEventType.GamepadTouchpadMotion:
                handler.OnGamepadTouchpadMotion(sdlEvent.GTouchpad);
                break;
            case SdlEventType.GamepadTouchpadUp:
                handler.OnGamepadTouchpadUp(sdlEvent.GTouchpad);
                break;
            case SdlEventType.GamepadSensorUpdate:
                handler.OnGamepadSensorUpdate(sdlEvent.GSensor);
                break;
            case SdlEventType.GamepadUpdateComplete:
                handler.OnGamepadUpdateComplete(sdlEvent.GDevice);
                break;
            case SdlEventType.GamepadSteamHandleUpdated:
                handler.OnGamepadSteamHandleUpdated(sdlEvent.GDevice);
                break;
            case SdlEventType.FingerDown:
                handler.OnFingerDown(sdlEvent.TFinger);
                break;
            case SdlEventType.FingerUp:
                handler.OnFingerUp(sdlEvent.TFinger);
                break;
            case SdlEventType.FingerMotion:
                handler.OnFingerMotion(sdlEvent.TFinger);
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
                handler.OnAudioDeviceAdded(sdlEvent.ADevice);
                break;
            case SdlEventType.AudioDeviceRemoved:
                handler.OnAudioDeviceRemoved(sdlEvent.ADevice);
                break;
            case SdlEventType.AudioDeviceFormatChanged:
                handler.OnAudioDeviceFormatChanged(sdlEvent.ADevice);
                break;
            case SdlEventType.SensorUpdate:
                handler.OnSensorUpdate(sdlEvent.Sensor);
                break;
            case SdlEventType.PenDown:
                handler.OnPenDown(sdlEvent.PTip);
                break;
            case SdlEventType.PenUp:
                handler.OnPenUp(sdlEvent.PTip);
                break;
            case SdlEventType.PenMotion:
                handler.OnPenMotion(sdlEvent.PMotion);
                break;
            case SdlEventType.PenButtonDown:
                handler.OnPenButtonDown(sdlEvent.PButton);
                break;
            case SdlEventType.PenButtonUp:
                handler.OnPenButtonUp(sdlEvent.PButton);
                break;
            case SdlEventType.RenderTargetsReset:
                handler.OnRenderTargetsReset();
                break;
            case SdlEventType.RenderDeviceReset:
                handler.OnRenderDeviceReset();
                break;
            default:
                if (SdlEventType.UserEvent <= sdlEvent.Type && sdlEvent.Type <= SdlEventType.LastEvent)
                    handler.OnUserEvent(sdlEvent.User);
                break;
        }
    }

    [InlineArray(128)]
    private struct PaddingArray
    {
        private byte _a;
    }
}
