using System;

namespace Piranha.Jawbone.Net;

[Flags]
public enum AddressType
{
    None,
    V4 = 1 << 0,
    V6 = 1 << 1
}
