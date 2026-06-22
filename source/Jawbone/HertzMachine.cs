using System;
using System.Diagnostics;

namespace Jawbone;

public struct HertzMachine
{
    private readonly long _frameLength;
    private readonly int _remainder;
    private readonly int _hertz;
    private long _nextFrame;
    private int _frameIndex;
    
    public long UpdateCount { get; private set; }

    public HertzMachine(int hertz) : this(hertz, Stopwatch.GetTimestamp())
    {
    }

    public HertzMachine(int hertz, long nextFrame)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(hertz, 1);
        _hertz = hertz;
        _frameLength = checked(Stopwatch.Frequency / hertz);
        _remainder = checked((int)(Stopwatch.Frequency % hertz));
        _nextFrame = nextFrame;
    }

    public bool TryAdvance() => TryAdvance(Stopwatch.GetTimestamp());

    public bool TryAdvance(long now)
    {
        if (_hertz < 1)
            return false;
        if (_nextFrame <= now)
        {
            var frameLength = _frameLength + Convert.ToInt64(_frameIndex < _remainder);
            _nextFrame += frameLength;
            _frameIndex = (_frameIndex + 1) % _hertz;
            ++UpdateCount;
            return true;
        }
        else
        {
            return false;
        }
    }
}