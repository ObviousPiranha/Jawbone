using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Tools;

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

    public bool Equals(Vector2<T> other) => X.Equals(other.X) && Y.Equals(other.Y);
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector2<T> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string? ToString() => $"{X}, ${Y}";
}
