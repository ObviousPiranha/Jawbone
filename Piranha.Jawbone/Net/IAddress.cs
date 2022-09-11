using System;
using System.Text;

namespace Piranha.Jawbone.Net;

public interface IAddress<TAddress> : IEquatable<TAddress>
{
    bool IsDefault { get; }
    void AppendTo(StringBuilder builder);
}