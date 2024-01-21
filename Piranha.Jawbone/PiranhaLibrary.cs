using Piranha.Jawbone.Generation;

namespace Piranha.Jawbone;

// https://www.sqlite.org/c3ref/funclist.html
[MapNativeFunctions]
public sealed partial class PiranhaLibrary
{
    public partial void Free(nint mem);
}
