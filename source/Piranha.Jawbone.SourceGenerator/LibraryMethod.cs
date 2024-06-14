using System.Collections.Generic;

namespace Piranha.Jawbone.SourceGenerator;

class LibraryMethod
{
    public string MethodName { get; set; } = "";
    public string FunctionPointerName => "_fp_" + MethodName;
    public string ReturnType { get; set; } = "";
    public List<LibraryParameter> Parameters { get; } = [];
}
