using System;
using System.Runtime.Serialization;

namespace Piranha.Jawbone.OpenGl
{
    public class OpenGlException : Exception
    {
        public OpenGlException()
        {
        }

        public OpenGlException(string? message) : base(message)
        {
        }

        public OpenGlException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OpenGlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
