using System;

namespace Jawbone.Sqlite;

public class MissingTableException : Exception
{
    public MissingTableException(string message) : base(message)
    { }
}
