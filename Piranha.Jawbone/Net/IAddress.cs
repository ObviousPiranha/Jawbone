using System;
using System.Text;

namespace Piranha.Jawbone.Net;

public interface IAddress<TAddress> : IEquatable<TAddress>
{
    void AppendTo(StringBuilder builder);
}