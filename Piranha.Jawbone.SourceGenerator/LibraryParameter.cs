namespace Piranha.Jawbone.SourceGenerator;

class LibraryParameter
{
    public string Name { get; set; } = "";
    public string FullDefinition { get; set; } = "";
    public bool PassesByReference { get; set; }

    public string GetFinalName()
    {
        return PassesByReference ? "__p_" + Name : Name;
    }
}