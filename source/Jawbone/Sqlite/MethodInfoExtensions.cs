using System;
using System.Reflection;

namespace Jawbone.Sqlite;

static class MethodInfoExtensions
{
    // https://github.com/dotnet/runtime/issues/30800
    // Coming... someday? Not even tagged for .NET 5. :(
    public static T CreateDelegate<T>(this MethodInfo method) where T : Delegate
    {
        return (T)Delegate.CreateDelegate(typeof(T), method);
    }
}
