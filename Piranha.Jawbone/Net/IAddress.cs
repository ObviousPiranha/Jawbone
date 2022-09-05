using System;
using System.Text;

namespace Piranha.Jawbone.Net;

public interface IAddress<T> : IEquatable<T>
{
    void AppendTo(StringBuilder builder);
}