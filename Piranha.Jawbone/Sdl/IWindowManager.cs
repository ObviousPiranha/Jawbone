namespace Piranha.Jawbone.Sdl
{
    public interface IWindowManager
    {
        void AddWindow(
            string title,
            int width,
            int height,
            bool fullscreen,
            IWindowEventHandler handler);
        
        bool TryExpose(uint windowId);
        
        void Run(IWindowManagerMetricHandler? windowManagerMetricHandler = null);
    }
}
