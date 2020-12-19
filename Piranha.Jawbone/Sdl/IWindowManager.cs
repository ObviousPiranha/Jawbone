namespace Piranha.Jawbone.Sdl
{
    public interface IWindowManager
    {
        uint AddWindow(
            string title,
            int width,
            int height,
            IWindowEventHandler handler);
        
        void Run();
    }
}
