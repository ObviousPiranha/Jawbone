using System;

namespace Piranha.Jawbone.Sdl;
public interface IWindowEventHandler
{
    void OnWindowCreated(Window window) { }
    void OnMouseEnter(Window window) { }
    void OnMouseLeave(Window window) { }
    void OnInputFocus(Window window) { }
    void OnInputBlur(Window window) { }
    void OnRestore(Window window) { }
    void OnMinimize(Window window) { }
    void OnMaximize(Window window) { }
    void OnShown(Window window) { }
    void OnHidden(Window window) { }
    void OnExpose(Window window) { }
    void OnLoop(Window window) { }
    void OnMove(Window window, SdlWindowEvent eventData) { }
    void OnResize(Window window, SdlWindowEvent eventData) { }
    void OnSizeChanged(Window window, SdlWindowEvent eventData) { }
    void OnKeyDown(Window window, SdlKeyboardEvent eventData) { }
    void OnKeyUp(Window window, SdlKeyboardEvent eventData) { }
    void OnTextInput(Window window, SdlTextInputEvent eventData) { }
    void OnMouseMove(Window window, SdlMouseMotionEvent eventData) { }
    void OnMouseWheel(Window window, SdlMouseWheelEvent eventData) { }
    void OnMouseButtonDown(Window window, SdlMouseButtonEvent eventData) { }
    void OnMouseButtonUp(Window window, SdlMouseButtonEvent eventData) { }
    void OnUser(Window window, SdlUserEvent eventdata) { }
    void OnClose(Window window) { }
    void OnQuit(Window window) { }
    void OnSecond(Window window) { }
    void OnDestroyingWindow(Window window) { }
}
