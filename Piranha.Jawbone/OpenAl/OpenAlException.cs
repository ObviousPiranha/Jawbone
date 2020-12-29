using System;
using System.Runtime.Serialization;

namespace Piranha.Jawbone.OpenAl
{
    [Serializable]
    public class OpenAlException : Exception
    {
        public OpenAlException()
        {
        }

        public OpenAlException(string? message) : base(message)
        {
        }

        public OpenAlException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OpenAlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
