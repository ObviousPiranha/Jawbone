using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone;

public readonly struct SlowFinder
{
    public readonly long Counter;
    public readonly long Longest;
    public readonly int LineNumber;

    public TimeSpan LongestTimespan => TimeSpan.FromSeconds(Longest / (double)Stopwatch.Frequency);

    public SlowFinder(long counter, long longest, int lineNumber)
    {
        Counter = counter;
        Longest = longest;
        LineNumber = lineNumber;
    }

    public SlowFinder Next([CallerLineNumber] int lineNumber = 0)
    {
        var counter = Stopwatch.GetTimestamp();
        var gap = counter - Counter;

        return Longest < gap ?
            new(counter, gap, lineNumber) :
            new(counter, Longest, LineNumber);
    }

    public static SlowFinder Begin([CallerLineNumber] int lineNumber = 0)
    {
        return new SlowFinder(Stopwatch.GetTimestamp(), 0, lineNumber);
    }
}
