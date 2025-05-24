using System;
using System.Runtime.CompilerServices;

namespace Jawbone;

public static class ExceptionHelper
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static T ThrowInvalidValue<T>(string paramName)
    {
        // https://github.com/dotnet/runtime/issues/4381
        // https://github.com/dotnet/coreclr/pull/6103
        throw new ArgumentException($"Invalid value submitted.", paramName);
    }
}
