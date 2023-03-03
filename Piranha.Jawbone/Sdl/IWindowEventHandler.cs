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
    void OnMove(Window window, WindowEventView eventData) { }
    void OnResize(Window window, WindowEventView eventData) { }
    void OnSizeChanged(Window window, WindowEventView eventData) { }
    void OnKeyDown(Window window, KeyboardEventView eventData) { }
    void OnKeyUp(Window window, KeyboardEventView eventData) { }
    void OnMouseMove(Window window, MouseMotionEventView eventData) { }
    void OnMouseWheel(Window window, MouseWheelEventView eventData) { }
    void OnMouseButtonDown(Window window, MouseButtonEventView eventData) { }
    void OnMouseButtonUp(Window window, MouseButtonEventView eventData) { }
    void OnUser(Window window, UserEventView eventdata) { }
    void OnClose(Window window) { }
    void OnQuit(Window window) { }
    void OnSecond(Window window) { }
    void OnDestroyingWindow(Window window) { }
}
