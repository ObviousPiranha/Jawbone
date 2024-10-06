namespace Piranha.Jawbone.Sdl3;

public struct SdlCameraSpec // SDL_CameraSpec
{
    public SdlPixelFormat Format; // SDL_PixelFormat format
    public SdlColorspace Colorspace; // SDL_Colorspace colorspace
    public int Width; // int width
    public int Height; // int height
    public int FramerateNumerator; // int framerate_numerator
    public int FramerateDenominator; // int framerate_denominator
}
