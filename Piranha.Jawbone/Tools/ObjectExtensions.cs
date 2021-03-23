using System;

namespace Piranha.Jawbone.Tools.ObjectExtensions
{
    public static class ObjectExtensions
    {
        public static T ThrowIfNull<T>(this T? value) where T : class
        {
            if (value is null)
                throw new NullReferenceException();
            
            return value;
        }
    }
}
