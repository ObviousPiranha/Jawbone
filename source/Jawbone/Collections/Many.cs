using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Jawbone;

static class Many
{
    public static void SetOrRemove<TKey, TValue>(
        Dictionary<TKey, ImmutableArray<TValue>> dictionary,
        TKey key,
        ImmutableArray<TValue> array) where TKey : notnull
    {
        Debug.Assert(!array.IsDefault);
        if (array.IsEmpty)
            dictionary.Remove(key);
        else
            dictionary[key] = array;
    }
}
