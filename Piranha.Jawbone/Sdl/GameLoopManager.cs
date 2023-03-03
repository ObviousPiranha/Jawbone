using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;

namespace Piranha.Jawbone.Sdl;

public sealed class GameLoopManager : IDisposable
{
    private readonly Thread _thread;
    private readonly ILogger<GameLoopManager> _logger;
    private readonly ISdl2 _sdl;
    private readonly IGameLoop _gameLoop;
    private bool _running = true;

    public GameLoopManager(
        ILogger<GameLoopManager> logger,
        ISdl2 sdl,
        IGameLoop gameLoop)
    {
        _logger = logger;
        _sdl = sdl;
        _gameLoop = gameLoop;

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
            int hertz = 60;
            var second = Stopwatch.Frequency;
            var shortFrame = second / hertz;
            var longFrameCount = (int)(second % hertz);

            var nextFrame = Stopwatch.GetTimestamp();
            var nextSecond = nextFrame + second;
            int frameIndex = 0;
            bool wasPrepared = true;

            while (_running && _gameLoop.Running)
            {
                var now = Stopwatch.GetTimestamp();

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
                    }

                    _gameLoop.BetweenFrames();
                }
                else
                {
                    _gameLoop.FrameUpdate();
                    wasPrepared = false;

                    // Stretch the leftover sub-frame across all the other frames.
                    nextFrame += shortFrame + Convert.ToInt64(frameIndex < longFrameCount);
                    frameIndex = (frameIndex + 1) % hertz;
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
