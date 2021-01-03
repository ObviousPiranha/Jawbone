using System;

namespace Piranha.Jawbone.Tools
{
    public static class ExceptionHelper
    {
        public static T ThrowInvalidValue<T>(string paramName)
        {
            throw new ArgumentException($"Invalid value submitted.", paramName);
        }
    }
}
