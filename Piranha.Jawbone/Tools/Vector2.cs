using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct Vector2<T> : IEquatable<Vector2<T>> where T : unmanaged, IEquatable<T>
{
    public T X;
    public T Y;

    public Vector2(T x, T y)
    {
        X = x;
        Y = y;
    }

    public readonly bool Equals(Vector2<T> other) => X.Equals(other.X) && Y.Equals(other.Y);
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Vector2<T> other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);
    public override readonly string ToString() => $"{X}, ${Y}";

    public static bool operator ==(Vector2<T> a, Vector2<T> b) => a.Equals(b);
    public static bool operator !=(Vector2<T> a, Vector2<T> b) => !a.Equals(b);
}
