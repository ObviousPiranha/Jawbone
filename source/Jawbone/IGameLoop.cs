namespace Jawbone;

public interface IGameLoop
{
    bool Running => true;
    void PrepareScene() { }
    void FrameUpdate() { }
    void BetweenFrames() { }
    void SecondUpdate() { }
    void Close() { }
}
