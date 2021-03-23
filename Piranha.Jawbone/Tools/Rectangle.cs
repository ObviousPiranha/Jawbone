using System;

namespace Piranha.Jawbone.Tools
{
    public readonly struct Rectangle<T> : IEquatable<Rectangle<T>> where T : IEquatable<T>
    {
        public readonly T A;
        public readonly T C;

        public Rectangle(T a, T c)
        {
            A = a;
            C = c;
        }

        public bool Equals(Rectangle<T> other) => A.Equals(other.A) && C.Equals(other.C);
        public override bool Equals(object? obj) => obj is Rectangle<T> other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(A, C);
        public override string? ToString() => A + " to " + C;
    }
}
