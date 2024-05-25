namespace Piranha.Jawbone.Sdl2;

public interface ISdlEventHandler
{
    bool Running => true;
    void OnSecond() { }
    void OnLoop() { }

    void OnQuit() { }
    void OnAppTerminating() { }
    void OnAppLowMemory() { }
    void OnAppWillEnterBackground() { }
    void OnAppDidEnterBackground() { }
    void OnAppWillEnterForeground() { }
    void OnAppDidEnterForeground() { }
    void OnLocaleChanged() { }
    void OnDisplayConnected(SdlDisplayEvent sdlEvent) { }
    void OnDisplayDisconnected(SdlDisplayEvent sdlEvent) { }
    void OnDisplayOrientation(SdlDisplayEvent sdlEvent) { }
    void OnWindowShown(SdlWindowEvent sdlEvent) { }
    void OnWindowHidden(SdlWindowEvent sdlEvent) { }
    void OnWindowExposed(SdlWindowEvent sdlEvent) { }
    void OnWindowMoved(SdlWindowEvent sdlEvent) { }
    void OnWindowResized(SdlWindowEvent sdlEvent) { }
    void OnWindowSizeChanged(SdlWindowEvent sdlEvent) { }
    void OnWindowMinimized(SdlWindowEvent sdlEvent) { }
    void OnWindowMaximized(SdlWindowEvent sdlEvent) { }
    void OnWindowRestored(SdlWindowEvent sdlEvent) { }
    void OnWindowEnter(SdlWindowEvent sdlEvent) { }
    void OnWindowLeave(SdlWindowEvent sdlEvent) { }
    void OnWindowFocusGained(SdlWindowEvent sdlEvent) { }
    void OnWindowFocusLost(SdlWindowEvent sdlEvent) { }
    void OnWindowClose(SdlWindowEvent sdlEvent) { }
    void OnWindowTakeFocus(SdlWindowEvent sdlEvent) { }
    void OnWindowHitTest(SdlWindowEvent sdlEvent) { }
    void OnWindowIccProfChanged(SdlWindowEvent sdlEvent) { }
    void OnWindowDisplayChanged(SdlWindowEvent sdlEvent) { }
    void OnSysWmEvent(SdlSysWmEvent sdlEvent) { }
    void OnKeyDown(SdlKeyboardEvent sdlEvent) { }
    void OnKeyUp(SdlKeyboardEvent sdlEvent) { }
    void OnTextEditing(SdlTextEditingEvent sdlEvent) { }
    void OnTextInput(SdlTextInputEvent sdlEvent) { }
    void OnKeyMapChanged() { }
    void OnTextEditingExt(SdlTextEditingExtEvent sdlEvent) { }
    void OnMouseMotion(SdlMouseMotionEvent sdlEvent) { }
    void OnMouseButtonDown(SdlMouseButtonEvent sdlEvent) { }
    void OnMouseButtonUp(SdlMouseButtonEvent sdlEvent) { }
    void OnMouseWheel(SdlMouseWheelEvent sdlEvent) { }
    void OnJoyAxisMotion(SdlJoyAxisEvent sdlEvent) { }
    void OnJoyBallMotion(SdlJoyBallEvent sdlEvent) { }
    void OnJoyHatMotion(SdlJoyHatEvent sdlEvent) { }
    void OnJoyButtonDown(SdlJoyButtonEvent sdlEvent) { }
    void OnJoyButtonUp(SdlJoyButtonEvent sdlEvent) { }
    void OnJoyDeviceAdded(SdlJoyDeviceEvent sdlEvent) { }
    void OnJoyDeviceRemoved(SdlJoyDeviceEvent sdlEvent) { }
    void OnJoyBatteryUpdated(SdlJoyBatteryEvent sdlEvent) { }
    void OnControllerAxisMotion(SdlControllerAxisEvent sdlEvent) { }
    void OnControllerButtonDown(SdlControllerButtonEvent sdlEvent) { }
    void OnControllerButtonUp(SdlControllerButtonEvent sdlEvent) { }
    void OnControllerDeviceAdded(SdlControllerDeviceEvent sdlEvent) { }
    void OnControllerDeviceRemoved(SdlControllerDeviceEvent sdlEvent) { }
    void OnControllerDeviceRemapped(SdlControllerDeviceEvent sdlEvent) { }
    void OnControllerTouchpadDown(SdlControllerTouchpadEvent sdlEvent) { }
    void OnControllerTouchpadMotion(SdlControllerTouchpadEvent sdlEvent) { }
    void OnControllerTouchpadUp(SdlControllerTouchpadEvent sdlEvent) { }
    void OnControllerTouchpadUpdate(SdlControllerTouchpadEvent sdlEvent) { }
    void OnFingerDown(SdlTouchFingerEvent sdlEvent) { }
    void OnFingerUp(SdlTouchFingerEvent sdlEvent) { }
    void OnFingerMotion(SdlTouchFingerEvent sdlEvent) { }
    void OnDollarGesture(SdlDollarGestureEvent sdlEvent) { }
    void OnDollarRecord(SdlDollarGestureEvent sdlEvent) { }
    void OnMultigesture(SdlMultiGestureEvent sdlEvent) { }
    void OnClipboardUpdate() { }
    void OnDropFile(SdlDropEvent sdlEvent) { }
    void OnDropText(SdlDropEvent sdlEvent) { }
    void OnDropBegin(SdlDropEvent sdlEvent) { }
    void OnDropComplete(SdlDropEvent sdlEvent) { }
    void OnAudioDeviceAdded(SdlAudioDeviceEvent sdlEvent) { }
    void OnAudioDeviceRemoved(SdlAudioDeviceEvent sdlEvent) { }
    void OnSensorUpdate(SdlSensorEvent sdlEvent) { }
    void OnRenderTargetsReset() { }
    void OnRenderDeviceReset() { }
    void OnUserEvent(SdlUserEvent sdlEvent) { }
}
