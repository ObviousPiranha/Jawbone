using Piranha.Jawbone.OpenGl;

namespace Piranha.Jawbone.Sdl
{
    public interface IWindowEventHandler
    {
        bool Running { get; }
        int ExposeVersionId { get; }

        void RequestExpose();
        void OnOpen(uint windowId, int width, int height, IOpenGl gl);
        void OnMouseEnter();
        void OnMouseLeave();
        void OnInputFocus();
        void OnInputBlur();
        void OnRestore();
        void OnMinimize();
        void OnMaximize();
        void OnExpose(IOpenGl gl);
        void OnMove(WindowEventView eventData);
        void OnResize(WindowEventView eventData);
        void OnSizeChanged(WindowEventView eventData);
        void OnKeyDown(KeyboardEventView eventData);
        void OnKeyUp(KeyboardEventView eventData);
        void OnMouseMove(MouseMotionEventView eventData);
        void OnMouseWheel(MouseWheelEventView eventData);
        void OnMouseButtonDown(MouseButtonEventView eventData);
        void OnMouseButtonUp(MouseButtonEventView eventData);
        bool OnUser(UserEventView eventdata);
        void OnClose();
        void OnQuit();
        void OnSecond();
    }
}
