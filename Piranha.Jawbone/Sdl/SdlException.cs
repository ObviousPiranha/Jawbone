using System;

namespace Piranha.Jawbone.Sdl
{
    public class SdlException : Exception
    {
        public SdlException(string message) : base(message)
        {
        }

        public SdlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
