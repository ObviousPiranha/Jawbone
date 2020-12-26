using System;

namespace Piranha.Jawbone.Tools
{
    public static class Comparer
    {
        public static bool IsLessThan(ReadOnlySpan<int> a, ReadOnlySpan<int> b)
        {
            if (a.Length != b.Length)
                return false;
            
            for (int i = 0; i < a.Length; ++i)
            {
                if (a[i] < b[i])
                    return true;
                else if (b[i] < a[i])
                    return false;
            }

            return false;
        }
    }
}