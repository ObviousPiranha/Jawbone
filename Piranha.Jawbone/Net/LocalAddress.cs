using System.ComponentModel;

namespace Piranha.Jawbone.Net;

[EditorBrowsable(EditorBrowsableState.Never)]
public readonly struct LocalAddress
{
    public static implicit operator Address32(LocalAddress _) => Address32.Local;
    public static implicit operator Address128(LocalAddress _) => Address128.Local;
}
