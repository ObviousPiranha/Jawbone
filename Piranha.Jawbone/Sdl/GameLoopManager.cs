using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Piranha.Jawbone.Sdl
{
    public sealed class GameLoopManager : IDisposable
    {
        private readonly Thread _thread;
        private readonly ILogger<GameLoopManager> _logger;
        private readonly ISdl2 _sdl;
        private readonly IGameLoop _gameLoop;
        private readonly IWindowEventHandler _windowEventHandler;
        private bool _running = true;

        public GameLoopManager(
            ILogger<GameLoopManager> logger,
            ISdl2 sdl,
            IGameLoop gameLoop,
            IWindowEventHandler windowEventHandler)
        {
            _logger = logger;
            _sdl = sdl;
            _gameLoop = gameLoop;
            _windowEventHandler = windowEventHandler;

            _thread = new Thread(RunGameLoopInBackground);
            _thread.Start();
        }

        public void Dispose()
        {
            _running = false;
            _thread.Join();
        }

        private void RunGameLoopInBackground()
        {
            try
            {
                var eventData = new byte[64];
                var userEventView = new UserEventView(eventData);
                int hertz = 60;
                var hertz64 = (ulong)hertz;
                ulong second = _sdl.GetPerformanceFrequency();
                ulong shortFrame = second / hertz64;
                var longFrameCount = (int)(second - (shortFrame * hertz64));

                ulong nextFrame = _sdl.GetPerformanceCounter();
                ulong nextSecond = nextFrame + second;
                var denominator = second / 1000.0;
                // var updateMilliseconds = new List<double>(64);
                // var renderMilliseconds = new List<double>(64);
                int frameIndex = 0;
                bool wasPrepared = true;

                while (_running && _gameLoop.Running)
                {
                    ulong now = _sdl.GetPerformanceCounter();

                    if (nextSecond <= now)
                    {
                        // TODO: Add metric reporting
                        nextSecond += second;
                    }

                    // Erase all the empty seconds (due to a long application pause).
                    while (nextSecond <= now)
                        nextSecond += second;

                    if (now < nextFrame)
                    {
                        if (wasPrepared)
                        {
                            // TODO: Pick the optimal sleep strat.
                            // https://randomascii.wordpress.com/2012/06/05/in-praise-of-idleness/
                            Thread.Sleep(1);
                        }
                        else
                        {
                            _gameLoop.PrepareScene();
                            wasPrepared = true;
                            _windowEventHandler.RequestExpose();
                            // renderMilliseconds.Add((_sdl.GetPerformanceCounter() - now) / denominator);
                        }
                    }
                    else
                    {
                        _gameLoop.FrameUpdate();
                        wasPrepared = false;
                        // updateMilliseconds.Add((_sdl.GetPerformanceCounter() - now) / denominator);

                        // Stretch the leftover sub-frame across all the other frames.
                        nextFrame += shortFrame + Convert.ToUInt64(frameIndex < longFrameCount);

                        if (++frameIndex >= hertz)
                            frameIndex = 0;
                    }
                }

                _gameLoop.Close();

                if (!_running)
                    _logger.LogDebug("Game loop exited gracefully via disposal.");
                else
                    _logger.LogDebug("Game loop exited gracefully via game loop.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in game loop.");
            }
        }
    }
}
