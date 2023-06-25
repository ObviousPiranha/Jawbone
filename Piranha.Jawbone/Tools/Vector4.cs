using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct Vector4<T> : IEquatable<Vector4<T>> where T : unmanaged, IEquatable<T>
{
    public T X;
    public T Y;
    public T Z;
    public T W;

    public Vector4(T x, T y, T z, T w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public readonly bool Equals(Vector4<T> other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Vector4<T> other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);
    public override readonly string ToString() => $"{X}, ${Y}, ${Z}, ${W}";

    public static bool operator ==(Vector4<T> a, Vector4<T> b) => a.Equals(b);
    public static bool operator !=(Vector4<T> a, Vector4<T> b) => !a.Equals(b);
}
