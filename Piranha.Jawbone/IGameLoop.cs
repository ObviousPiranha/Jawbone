namespace Piranha.Jawbone;

public interface IGameLoop
{
    bool Running { get => true; }
    void PrepareScene() { }
    void FrameUpdate() { }
    void BetweenFrames() { }
    void SecondUpdate() { }
    void Close() { }
}
