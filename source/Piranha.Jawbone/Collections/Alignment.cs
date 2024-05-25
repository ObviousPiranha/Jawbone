using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

public static class Alignment
{
    private static class AlignmentInfo<T> where T : unmanaged
    {
#pragma warning disable CS0649
        private struct Helper
        {
            public byte padding;
            public T target;
        }
#pragma warning restore CS0649

        public static readonly int Value = GetAlignment();

        private static int GetAlignment()
        {
            return (int)Marshal.OffsetOf<Helper>(nameof(Helper.target));
        }
    }

    public static int Of<T>() where T : unmanaged
    {
        return AlignmentInfo<T>.Value;
    }
}
