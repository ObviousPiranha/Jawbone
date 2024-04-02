using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

[StructLayout(LayoutKind.Explicit)]
public struct SdlEvent
{
    [FieldOffset(0)] public SdlEventType Type;
    [FieldOffset(0)] public SdlCommonEvent Common;
    [FieldOffset(0)] public SdlDisplayEvent Display;
    [FieldOffset(0)] public SdlWindowEvent Window;
    [FieldOffset(0)] public SdlKeyboardEvent Key;
    [FieldOffset(0)] public SdlTextEditingEvent Edit;
    [FieldOffset(0)] public SdlTextEditingExtEvent EditExt;
    [FieldOffset(0)] public SdlTextInputEvent Text;
    [FieldOffset(0)] public SdlMouseMotionEvent Motion;
    [FieldOffset(0)] public SdlMouseButtonEvent Button;
    [FieldOffset(0)] public SdlMouseWheelEvent Wheel;
    [FieldOffset(0)] public SdlJoyAxisEvent JAxis;
    [FieldOffset(0)] public SdlJoyBallEvent JBall;
    [FieldOffset(0)] public SdlJoyHatEvent JHat;
    [FieldOffset(0)] public SdlJoyButtonEvent JButton;
    [FieldOffset(0)] public SdlJoyDeviceEvent JDevice;
    [FieldOffset(0)] public SdlJoyBatteryEvent JBattery;
    [FieldOffset(0)] public SdlControllerAxisEvent CAxis;
    [FieldOffset(0)] public SdlControllerButtonEvent CButton;
    [FieldOffset(0)] public SdlControllerDeviceEvent CDevice;
    [FieldOffset(0)] public SdlControllerTouchpadEvent CTouchpad;
    [FieldOffset(0)] public SdlControllerSensorEvent CSensor;
    [FieldOffset(0)] public SdlAudioDeviceEvent ADevice;
    [FieldOffset(0)] public SdlSensorEvent Sensor;
    [FieldOffset(0)] public SdlQuitEvent Quit;
    [FieldOffset(0)] public SdlUserEvent User;
    [FieldOffset(0)] public SdlSysWmEvent SysWm;
    [FieldOffset(0)] public SdlTouchFingerEvent TFinger;
    [FieldOffset(0)] public SdlMultiGestureEvent MGesture;
    [FieldOffset(0)] public SdlDollarGestureEvent DGesture;
    [FieldOffset(0)] public SdlDropEvent Drop;

    public static void Dispatch(Sdl3Library sdl, in SdlEvent sdlEvent, ISdlEventHandler handler)
    {
        switch (sdlEvent.Type)
        {
            case SdlEventType.Quit:
                handler.OnQuit();
                break;
            case SdlEventType.AppTerminating:
                handler.OnAppTerminating();
                break;
            case SdlEventType.AppLowMemory:
                handler.OnAppLowMemory();
                break;
            case SdlEventType.AppWillEnterBackground:
                handler.OnAppWillEnterBackground();
                break;
            case SdlEventType.AppDidEnterBackground:
                handler.OnAppDidEnterBackground();
                break;
            case SdlEventType.AppWillEnterForeground:
                handler.OnAppWillEnterForeground();
                break;
            case SdlEventType.AppDidEnterForeground:
                handler.OnAppDidEnterForeground();
                break;
            case SdlEventType.LocaleChanged:
                handler.OnLocaleChanged();
                break;
            case SdlEventType.DisplayEvent:
                Dispatch(sdlEvent.Display, handler);
                break;
            case SdlEventType.WindowEvent:
                Dispatch(sdlEvent.Window, handler);
                break;
            case SdlEventType.SysWmEvent:
                handler.OnSysWmEvent(sdlEvent.SysWm);
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
            case SdlEventType.TextEditingExt:
                handler.OnTextEditingExt(sdlEvent.EditExt);
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
            case SdlEventType.JoyAxisMotion:
                handler.OnJoyAxisMotion(sdlEvent.JAxis);
                break;
            case SdlEventType.JoyBallMotion:
                handler.OnJoyBallMotion(sdlEvent.JBall);
                break;
            case SdlEventType.JoyHatMotion:
                handler.OnJoyHatMotion(sdlEvent.JHat);
                break;
            case SdlEventType.JoyButtonDown:
                handler.OnJoyButtonDown(sdlEvent.JButton);
                break;
            case SdlEventType.JoyButtonUp:
                handler.OnJoyButtonUp(sdlEvent.JButton);
                break;
            case SdlEventType.JoyDeviceAdded:
                handler.OnJoyDeviceAdded(sdlEvent.JDevice);
                break;
            case SdlEventType.JoyDeviceRemoved:
                handler.OnJoyDeviceRemoved(sdlEvent.JDevice);
                break;
            case SdlEventType.JoyBatteryUpdated:
                handler.OnJoyBatteryUpdated(sdlEvent.JBattery);
                break;
            case SdlEventType.ControllerAxisMotion:
                handler.OnControllerAxisMotion(sdlEvent.CAxis);
                break;
            case SdlEventType.ControllerButtonDown:
                handler.OnControllerButtonDown(sdlEvent.CButton);
                break;
            case SdlEventType.ControllerButtonUp:
                handler.OnControllerButtonUp(sdlEvent.CButton);
                break;
            case SdlEventType.ControllerDeviceAdded:
                handler.OnControllerDeviceAdded(sdlEvent.CDevice);
                break;
            case SdlEventType.ControllerDeviceRemoved:
                handler.OnControllerDeviceRemoved(sdlEvent.CDevice);
                break;
            case SdlEventType.ControllerDeviceRemapped:
                handler.OnControllerDeviceRemapped(sdlEvent.CDevice);
                break;
            case SdlEventType.ControllerTouchpadDown:
                handler.OnControllerTouchpadDown(sdlEvent.CTouchpad);
                break;
            case SdlEventType.ControllerTouchpadMotion:
                handler.OnControllerTouchpadMotion(sdlEvent.CTouchpad);
                break;
            case SdlEventType.ControllerTouchpadUp:
                handler.OnControllerTouchpadUp(sdlEvent.CTouchpad);
                break;
            case SdlEventType.ControllerTouchpadUpdate:
                handler.OnControllerTouchpadUpdate(sdlEvent.CTouchpad);
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
            case SdlEventType.DollarGesture:
                handler.OnDollarGesture(sdlEvent.DGesture);
                break;
            case SdlEventType.DollarRecord:
                handler.OnDollarRecord(sdlEvent.DGesture);
                break;
            case SdlEventType.Multigesture:
                handler.OnMultigesture(sdlEvent.MGesture);
                break;
            case SdlEventType.ClipboardUpdate:
                handler.OnClipboardUpdate();
                break;
            case SdlEventType.DropFile:
                handler.OnDropFile(sdlEvent.Drop);
                sdl.Free(sdlEvent.Drop.File);
                break;
            case SdlEventType.DropText:
                handler.OnDropText(sdlEvent.Drop);
                sdl.Free(sdlEvent.Drop.File);
                break;
            case SdlEventType.DropBegin:
                handler.OnDropBegin(sdlEvent.Drop);
                sdl.Free(sdlEvent.Drop.File);
                break;
            case SdlEventType.DropComplete:
                handler.OnDropComplete(sdlEvent.Drop);
                sdl.Free(sdlEvent.Drop.File);
                break;
            case SdlEventType.AudioDeviceAdded:
                handler.OnAudioDeviceAdded(sdlEvent.ADevice);
                break;
            case SdlEventType.AudioDeviceRemoved:
                handler.OnAudioDeviceRemoved(sdlEvent.ADevice);
                break;
            case SdlEventType.SensorUpdate:
                handler.OnSensorUpdate(sdlEvent.Sensor);
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

    private static void Dispatch(in SdlDisplayEvent sdlEvent, ISdlEventHandler handler)
    {
        switch (sdlEvent.Event)
        {
            case SdlDisplayEventType.Connected:
                handler.OnDisplayConnected(sdlEvent);
                break;
            case SdlDisplayEventType.Disconnected:
                handler.OnDisplayDisconnected(sdlEvent);
                break;
            case SdlDisplayEventType.Orientation:
                handler.OnDisplayOrientation(sdlEvent);
                break;
        }
    }

    private static void Dispatch(in SdlWindowEvent sdlEvent, ISdlEventHandler handler)
    {
        switch (sdlEvent.Event)
        {
            case SdlWindowEventType.Shown:
                handler.OnWindowShown(sdlEvent);
                break;
            case SdlWindowEventType.Hidden:
                handler.OnWindowHidden(sdlEvent);
                break;
            case SdlWindowEventType.Exposed:
                handler.OnWindowExposed(sdlEvent);
                break;
            case SdlWindowEventType.Moved:
                handler.OnWindowMoved(sdlEvent);
                break;
            case SdlWindowEventType.Resized:
                handler.OnWindowResized(sdlEvent);
                break;
            case SdlWindowEventType.SizeChanged:
                handler.OnWindowSizeChanged(sdlEvent);
                break;
            case SdlWindowEventType.Minimized:
                handler.OnWindowMinimized(sdlEvent);
                break;
            case SdlWindowEventType.Maximized:
                handler.OnWindowMaximized(sdlEvent);
                break;
            case SdlWindowEventType.Restored:
                handler.OnWindowRestored(sdlEvent);
                break;
            case SdlWindowEventType.Enter:
                handler.OnWindowEnter(sdlEvent);
                break;
            case SdlWindowEventType.Leave:
                handler.OnWindowLeave(sdlEvent);
                break;
            case SdlWindowEventType.FocusGained:
                handler.OnWindowFocusGained(sdlEvent);
                break;
            case SdlWindowEventType.FocusLost:
                handler.OnWindowFocusLost(sdlEvent);
                break;
            case SdlWindowEventType.Close:
                handler.OnWindowClose(sdlEvent);
                break;
            case SdlWindowEventType.TakeFocus:
                handler.OnWindowTakeFocus(sdlEvent);
                break;
            case SdlWindowEventType.HitTest:
                handler.OnWindowHitTest(sdlEvent);
                break;
            case SdlWindowEventType.IccProfChanged:
                handler.OnWindowIccProfChanged(sdlEvent);
                break;
            case SdlWindowEventType.DisplayChanged:
                handler.OnWindowDisplayChanged(sdlEvent);
                break;
        }
    }
}
