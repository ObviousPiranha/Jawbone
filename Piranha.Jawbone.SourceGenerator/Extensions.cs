using System;
using System.Text;

namespace Piranha.Jawbone.SourceGenerator;

static class Extensions
{
    public static T CannotBeNull<T>(this T? item) where T : class
    {
        if (item is null)
            throw new NullReferenceException();
        return item!;
    }

    public static StringBuilder Indent(this StringBuilder sb, IndentState state)
    {
        return sb.Append(' ', state.Total);
    }
}
