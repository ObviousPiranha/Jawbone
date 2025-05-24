namespace Jawbone.Sdl3;

public enum SdlHitTestResult // SDL_HitTestResult
{
    Normal = 0, // SDL_HITTEST_NORMAL
    Draggable = 1, // SDL_HITTEST_DRAGGABLE
    ResizeTopleft = 2, // SDL_HITTEST_RESIZE_TOPLEFT
    ResizeTop = 3, // SDL_HITTEST_RESIZE_TOP
    ResizeTopright = 4, // SDL_HITTEST_RESIZE_TOPRIGHT
    ResizeRight = 5, // SDL_HITTEST_RESIZE_RIGHT
    ResizeBottomright = 6, // SDL_HITTEST_RESIZE_BOTTOMRIGHT
    ResizeBottom = 7, // SDL_HITTEST_RESIZE_BOTTOM
    ResizeBottomleft = 8, // SDL_HITTEST_RESIZE_BOTTOMLEFT
    ResizeLeft = 9, // SDL_HITTEST_RESIZE_LEFT
}
