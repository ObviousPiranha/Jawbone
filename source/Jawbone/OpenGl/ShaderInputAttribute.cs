using System;

namespace Jawbone.OpenGl;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ShaderInputAttribute : Attribute
{
    public string Name { get; }
    public ShaderInputSettings Settings { get; }

    public ShaderInputAttribute(
        string name,
        ShaderInputSettings settings = ShaderInputSettings.None)
    {
        Name = name;
        Settings = settings;
    }
}
