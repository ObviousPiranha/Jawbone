using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl;

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
}
