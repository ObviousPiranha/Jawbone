using System;
using System.Diagnostics;
using System.Threading;

namespace Piranha.Jawbone.Sdl2;

public static class ApplicationManager
{
    public static void Run(Sdl2Library sdl, ISdlEventHandler eventHandler)
    {
        var nextSecond = Stopwatch.GetTimestamp() + Stopwatch.Frequency;

        while (eventHandler.Running)
        {
            var doSleep = true;
            while (sdl.PollEvent(out var sdlEvent) == 1)
            {
                SdlEvent.Dispatch(sdl, sdlEvent, eventHandler);
                doSleep = false;
            }

            eventHandler.OnLoop();

            var now = Stopwatch.GetTimestamp();
            if (nextSecond <= now)
            {
                doSleep = false;
                eventHandler.OnSecond();
                nextSecond += Stopwatch.Frequency;
            }

            if (doSleep)
            {
                //var stopwatch = ValueStopwatch.Start();
                Thread.Sleep(1);
            }
        }
    }

    public static void ToggleFullScreen(Sdl2Library sdl, nint window)
    {
        // https://superuser.com/a/1251294
        // http://lists.libsdl.org/pipermail/commits-libsdl.org/2018-January/002542.html
        // https://discourse.libsdl.org/t/cannot-remove-the-window-title-bar-and-borders/25615
        // https://discourse.libsdl.org/t/true-borderless-fullscreen-behaviour/24622

        int displayIndex = sdl.GetWindowDisplayIndex(window);
        if (displayIndex < 0)
            throw new SdlException("error getting window display");

        if (sdl.GetDisplayUsableBounds(displayIndex, out var rect) != 0)
            throw new SdlException("error getting usable bounds");

        var flag = sdl.GetWindowFlags(window) & SdlWindow.FullScreenDesktop;
        sdl.SetWindowFullscreen(window, flag ^ SdlWindow.FullScreenDesktop);
    }
}
