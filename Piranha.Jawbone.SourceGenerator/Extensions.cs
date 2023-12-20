using System.Collections.Generic;
using System.Linq;

namespace Piranha.Jawbone.SourceGenerator;

static class Extensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable) where T : class
    {
        return enumerable.Where(item => item is not null).Cast<T>();
    }
}
