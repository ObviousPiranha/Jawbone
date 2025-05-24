namespace Jawbone.Sdl3;

public struct SdlRect // SDL_Rect
{
    public int X; // int x
    public int Y; // int y
    public int W; // int w
    public int H; // int h

    public readonly override string ToString() => $"{X}, {Y} {W}x{H}";
}
