using System.Text;

namespace Piranha.Jawbone;

public static class PascalCase
{
    public static string ToSnakeCase(string prefix, string pascalCase)
    {
        var builder = new StringBuilder(pascalCase.Length * 2 + prefix.Length).Append(prefix);

        foreach (var c in pascalCase)
        {
            if (char.IsUpper(c))
                builder.Append('_').Append(char.ToLowerInvariant(c));
            else
                builder.Append(c);
        }
        
        return builder.ToString();
    }
}