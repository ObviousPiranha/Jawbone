using System;

namespace Piranha.Jawbone.Sqlite
{
    public class MissingTableException : Exception
    {
        public MissingTableException(string message) : base(message)
        {}
    }
}
