using System.Runtime.CompilerServices;

namespace Jawbone;

[InlineArray(4)]
public struct Four<T>
{
    private T _data;
}