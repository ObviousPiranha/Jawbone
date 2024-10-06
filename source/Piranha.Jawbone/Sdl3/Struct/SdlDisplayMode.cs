namespace Piranha.Jawbone.Sdl3;

public struct SdlDisplayMode // SDL_DisplayMode
{
    public uint DisplayID; // SDL_DisplayID displayID
    public SdlPixelFormat Format; // SDL_PixelFormat format
    public int W; // int w
    public int H; // int h
    public float PixelDensity; // float pixel_density
    public float RefreshRate; // float refresh_rate
    public int RefreshRateNumerator; // int refresh_rate_numerator
    public int RefreshRateDenominator; // int refresh_rate_denominator
    public nint Internal; // SDL_DisplayModeData * internal
}
