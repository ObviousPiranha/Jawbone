namespace Piranha.Jawbone.Sdl
{
    public interface IGameLoop
    {
        bool Running { get; }
        void PrepareScene();
        void FrameUpdate();
    }
}
