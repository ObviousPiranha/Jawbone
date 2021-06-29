using System;
using System.Diagnostics;

namespace Piranha.Jawbone.Tools
{
    public readonly struct ValueStopwatch : IEquatable<ValueStopwatch>
    {
        private static readonly double Multiplier = 1000d / Stopwatch.Frequency;

        private readonly long _timestamp;

        private ValueStopwatch(long timestamp) => _timestamp = timestamp;
        public bool Equals(ValueStopwatch other) => _timestamp == other._timestamp;
        public override bool Equals(object? obj) => obj is ValueStopwatch other && Equals(other);
        public override int GetHashCode() => _timestamp.GetHashCode();
        public override string? ToString() => _timestamp.ToString();

        public double GetElapsedMilliseconds() => (Stopwatch.GetTimestamp() - _timestamp) * Multiplier;
        public TimeSpan GetElapsed() => TimeSpan.FromMilliseconds(GetElapsedMilliseconds());

        public static ValueStopwatch Start() => new(Stopwatch.GetTimestamp());
    }
}
