namespace Piranha.Jawbone.SourceGenerator;

class LibraryParameter
{
    public string Name { get; set; } = "";
    public string MethodDefinition { get; set; } = "";
    public string FunctionPointerType { get; set; } = "";
    public bool PassesByReference { get; set; }

    public string GetFinalName()
    {
        return PassesByReference ? "__p_" + Name : Name;
    }
}
