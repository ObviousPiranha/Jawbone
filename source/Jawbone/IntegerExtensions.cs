namespace Jawbone.Extensions;

public static class IntegerExtensions
{
    public static bool MaskAll(this int n, int mask) => (n & mask) == mask;
    public static bool MaskAny(this int n, int mask) => (n & mask) != 0;
}
