using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone;

public readonly struct ValueStopwatch : IEquatable<ValueStopwatch>
{
    private readonly long _timestamp;

    public readonly bool Started => 0 < _timestamp;

    private ValueStopwatch(long timestamp) => _timestamp = timestamp;
    public readonly bool Equals(ValueStopwatch other) => _timestamp == other._timestamp;
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is ValueStopwatch other && Equals(other);
    public override readonly int GetHashCode() => _timestamp.GetHashCode();
    public override readonly string ToString() => _timestamp.ToString();

    public double GetElapsedMilliseconds() => GetElapsed().TotalMilliseconds;
    public TimeSpan GetElapsed() => Stopwatch.GetElapsedTime(_timestamp);

    public static ValueStopwatch Start() => new(Stopwatch.GetTimestamp());

    public static bool operator ==(ValueStopwatch a, ValueStopwatch b) => a.Equals(b);
    public static bool operator !=(ValueStopwatch a, ValueStopwatch b) => !a.Equals(b);
}
