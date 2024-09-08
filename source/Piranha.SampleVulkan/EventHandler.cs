using Piranha.Jawbone.Sdl2;

namespace Piranha.SampleVulkan;

class EventHandler : ISdlEventHandler
{
    public bool Running { get; private set; } = true;
    public void OnQuit() => Running = false;
    public void OnWindowClose(SdlWindowEvent sdlEvent) => Running = false;
}
