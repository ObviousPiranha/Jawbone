using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct Vector3<T> : IEquatable<Vector3<T>> where T : unmanaged, IEquatable<T>
{
    public T X;
    public T Y;
    public T Z;

    public Vector3(T x, T y, T z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public readonly bool Equals(Vector3<T> other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Vector3<T> other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);
    public override readonly string ToString() => $"{X}, ${Y}, ${Z}";

    public static explicit operator Vector3<T>(Vector4<T> v4) => new(v4.X, v4.Y, v4.Z);

    public static bool operator ==(Vector3<T> a, Vector3<T> b) => a.Equals(b);
    public static bool operator !=(Vector3<T> a, Vector3<T> b) => !a.Equals(b);
}
