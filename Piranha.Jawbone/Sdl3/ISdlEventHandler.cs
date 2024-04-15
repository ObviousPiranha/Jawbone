namespace Piranha.Jawbone.Sdl3;

public interface ISdlEventHandler
{
    bool Running => true;
    void OnSecond() { }
    void OnLoop() { }

    void OnQuit() { }
    void OnTerminating() { }
    void OnLowMemory() { }
    void OnWillEnterBackground() { }
    void OnDidEnterBackground() { }
    void OnWillEnterForeground() { }
    void OnDidEnterForeground() { }
    void OnLocaleChanged() { }
    void OnDisplayAdded(SdlDisplayEvent sdlEvent) { }
    void OnDisplayRemoved(SdlDisplayEvent sdlEvent) { }
    void OnDisplayOrientation(SdlDisplayEvent sdlEvent) { }
    void OnDisplayMoved(SdlDisplayEvent sdlEvent) { }
    void OnDisplayContentScaleChanged(SdlDisplayEvent sdlEvent) { }
    void OnDisplayHdrStateChanged(SdlDisplayEvent sdlEvent) { }
    void OnWindowShown(SdlWindowEvent sdlEvent) { }
    void OnWindowHidden(SdlWindowEvent sdlEvent) { }
    void OnWindowExposed(SdlWindowEvent sdlEvent) { }
    void OnWindowMoved(SdlWindowEvent sdlEvent) { }
    void OnWindowResized(SdlWindowEvent sdlEvent) { }
    void OnWindowPixelSizeChanged(SdlWindowEvent sdlEvent) { }
    void OnWindowMinimized(SdlWindowEvent sdlEvent) { }
    void OnWindowMaximized(SdlWindowEvent sdlEvent) { }
    void OnWindowRestored(SdlWindowEvent sdlEvent) { }
    void OnWindowMouseEnter(SdlWindowEvent sdlEvent) { }
    void OnWindowMouseLeave(SdlWindowEvent sdlEvent) { }
    void OnWindowFocusGained(SdlWindowEvent sdlEvent) { }
    void OnWindowFocusLost(SdlWindowEvent sdlEvent) { }
    void OnWindowCloseRequested(SdlWindowEvent sdlEvent) { }
    void OnWindowTakeFocus(SdlWindowEvent sdlEvent) { }
    void OnWindowHitTest(SdlWindowEvent sdlEvent) { }
    void OnWindowIccProfChanged(SdlWindowEvent sdlEvent) { }
    void OnWindowDisplayChanged(SdlWindowEvent sdlEvent) { }
    void OnWindowDisplayScaleChanged(SdlWindowEvent sdlEvent) { }
    void OnWindowOccluded(SdlWindowEvent sdlEvent) { }
    void OnWindowEnterFullscreen(SdlWindowEvent sdlEvent) { }
    void OnWindowLeaveFullscreen(SdlWindowEvent sdlEvent) { }
    void OnWindowDestroyed(SdlWindowEvent sdlEvent) { }
    void OnWindowPenEnter(SdlWindowEvent sdlEvent) { }
    void OnWindowPenLeave(SdlWindowEvent sdlEvent) { }
    void OnKeyDown(SdlKeyboardEvent sdlEvent) { }
    void OnKeyUp(SdlKeyboardEvent sdlEvent) { }
    void OnTextEditing(SdlTextEditingEvent sdlEvent) { }
    void OnTextInput(SdlTextInputEvent sdlEvent) { }
    void OnKeyboardAdded(SdlKeyboardDeviceEvent sdlEvent) { }
    void OnKeyboardRemoved(SdlKeyboardDeviceEvent sdlEvent) { }
    void OnKeyMapChanged() { }
    void OnMouseMotion(SdlMouseMotionEvent sdlEvent) { }
    void OnMouseButtonDown(SdlMouseButtonEvent sdlEvent) { }
    void OnMouseButtonUp(SdlMouseButtonEvent sdlEvent) { }
    void OnMouseWheel(SdlMouseWheelEvent sdlEvent) { }
    void OnMouseAdded(SdlMouseDeviceEvent sdlEvent) { }
    void OnMouseRemoved(SdlMouseDeviceEvent sdlEvent) { }
    void OnJoystickAxisMotion(SdlJoyAxisEvent sdlEvent) { }
    void OnJoystickBallMotion(SdlJoyBallEvent sdlEvent) { }
    void OnJoystickHatMotion(SdlJoyHatEvent sdlEvent) { }
    void OnJoystickButtonDown(SdlJoyButtonEvent sdlEvent) { }
    void OnJoystickButtonUp(SdlJoyButtonEvent sdlEvent) { }
    void OnJoystickAdded(SdlJoyDeviceEvent sdlEvent) { }
    void OnJoystickRemoved(SdlJoyDeviceEvent sdlEvent) { }
    void OnJoystickBatteryUpdated(SdlJoyBatteryEvent sdlEvent) { }
    void OnJoystickUpdateCompleted(SdlJoyDeviceEvent sdlEvent) { }
    void OnGamepadAxisMotion(SdlControllerAxisEvent sdlEvent) { }
    void OnGamepadButtonDown(SdlControllerButtonEvent sdlEvent) { }
    void OnGamepadButtonUp(SdlControllerButtonEvent sdlEvent) { }
    void OnGamepadDeviceAdded(SdlGamepadDeviceEvent sdlEvent) { }
    void OnGamepadDeviceRemoved(SdlGamepadDeviceEvent sdlEvent) { }
    void OnGamepadDeviceRemapped(SdlGamepadDeviceEvent sdlEvent) { }
    void OnGamepadTouchpadDown(SdlControllerTouchpadEvent sdlEvent) { }
    void OnGamepadTouchpadMotion(SdlControllerTouchpadEvent sdlEvent) { }
    void OnGamepadTouchpadUp(SdlControllerTouchpadEvent sdlEvent) { }
    void OnGamepadSensorUpdate(SdlGamepadSensorEvent sdlEvent) { }
    void OnGamepadUpdateComplete(SdlGamepadDeviceEvent sdlEvent) { }
    void OnGamepadSteamHandleUpdated(SdlGamepadDeviceEvent sdlEvent) { }
    void OnFingerDown(SdlTouchFingerEvent sdlEvent) { }
    void OnFingerUp(SdlTouchFingerEvent sdlEvent) { }
    void OnFingerMotion(SdlTouchFingerEvent sdlEvent) { }
    void OnClipboardUpdate(SdlClipboardEvent sdlEvent) { }
    void OnDropFile(SdlDropEvent sdlEvent) { }
    void OnDropText(SdlDropEvent sdlEvent) { }
    void OnDropBegin(SdlDropEvent sdlEvent) { }
    void OnDropComplete(SdlDropEvent sdlEvent) { }
    void OnDropPosition(SdlDropEvent sdlEvent) { }
    void OnAudioDeviceAdded(SdlAudioDeviceEvent sdlEvent) { }
    void OnAudioDeviceRemoved(SdlAudioDeviceEvent sdlEvent) { }
    void OnAudioDeviceFormatChanged(SdlAudioDeviceEvent sdlEvent) { }
    void OnSensorUpdate(SdlSensorEvent sdlEvent) { }
    void OnPenDown(SdlPenTipEvent sdlEvent) { }
    void OnPenUp(SdlPenTipEvent sdlEvent) { }
    void OnPenMotion(SdlPenMotionEvent sdlEvent) { }
    void OnPenButtonDown(SdlPenButtonEvent sdlEvent) { }
    void OnPenButtonUp(SdlPenButtonEvent sdlEvent) { }
    void OnCameraDeviceAdded(SdlCameraDeviceEvent sdlEvent) { }
    void OnCameraDeviceRemoved(SdlCameraDeviceEvent sdlEvent) { }
    void OnCameraDeviceApproved(SdlCameraDeviceEvent sdlEvent) { }
    void OnCameraDeviceDenied(SdlCameraDeviceEvent sdlEvent) { }
    void OnRenderTargetsReset() { }
    void OnRenderDeviceReset() { }
    void OnUserEvent(SdlUserEvent sdlEvent) { }
}
