using System.ComponentModel;

namespace Piranha.Jawbone.Net;

[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct AnyAddress
{
    public static implicit operator Address32(AnyAddress _) => Address32.Any;
    public static implicit operator Address128(AnyAddress _) => Address128.Any;
}
