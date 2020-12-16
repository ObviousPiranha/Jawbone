using System;

namespace Piranha.Sqlite
{
    public class MissingTableException : Exception
    {
        public MissingTableException(string message) : base(message)
        {}
    }
}