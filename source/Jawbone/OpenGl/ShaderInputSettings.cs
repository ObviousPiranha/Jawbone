using System;

namespace Jawbone.OpenGl;

[Flags]
public enum ShaderInputSettings
{
    None,
    Normalized = 1 << 0
}
