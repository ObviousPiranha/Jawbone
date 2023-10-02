using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

public static class Alignment
{
    private struct Helper<T> where T : unmanaged
    {
        public byte padding;
        public T target;
    }

    private static class AlignmentInfo<T> where T : unmanaged
    {
        public static readonly int Value = GetAlignment();

        private static int GetAlignment()
        {
            return (int)Marshal.OffsetOf<Helper<T>>(nameof(Helper<T>.target));
        }
    }

    public static int Of<T>() where T : unmanaged
    {
        return AlignmentInfo<T>.Value;
    }
}
