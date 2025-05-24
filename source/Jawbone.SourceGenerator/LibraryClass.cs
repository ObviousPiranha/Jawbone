using System.Collections.Generic;
using System.Text;

namespace Jawbone.SourceGenerator;

class LibraryClass
{
    public string ClassName { get; set; } = "";
    public string Namespace { get; set; } = "";
    public List<LibraryMethod> Methods { get; } = new();

    public string BuildSource()
    {
        var builder = new StringBuilder();
        return builder.ToString();
    }
}
