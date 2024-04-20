using System;
using System.Diagnostics;
using System.Threading;

namespace Piranha.Jawbone.Sdl3;

public static class ApplicationManager
{
    public static void Run(Sdl3Library sdl, ISdlEventHandler eventHandler)
    {
        var nextSecond = Stopwatch.GetTimestamp() + Stopwatch.Frequency;

        while (eventHandler.Running)
        {
            var doSleep = true;
            while (sdl.PollEvent(out var sdlEvent))
            {
                SdlEvent.Dispatch(sdlEvent, eventHandler);
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

        while (sdl.PollEvent(out var sdlEvent))
            SdlEvent.Dispatch(sdlEvent, eventHandler);
    }
}
