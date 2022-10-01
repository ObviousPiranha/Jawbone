using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Tools;

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

    public bool Equals(Vector3<T> other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector3<T> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    public override string? ToString() => $"{X}, ${Y}, ${Z}";

    public static bool operator ==(Vector3<T> a, Vector3<T> b) => a.Equals(b);
    public static bool operator !=(Vector3<T> a, Vector3<T> b) => !a.Equals(b);
}
