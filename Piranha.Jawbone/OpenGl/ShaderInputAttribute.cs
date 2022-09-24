using System;

namespace Piranha.Jawbone.OpenGl;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ShaderInputAttribute : Attribute
{
    public string Name { get; }

    public ShaderInputAttribute(string name) => Name = name;
}
