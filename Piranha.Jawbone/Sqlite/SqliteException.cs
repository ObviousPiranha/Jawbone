using System;
using System.Collections.Generic;

namespace Piranha.Jawbone.Sqlite
{
    public class SqliteException : Exception
    {
        public KeyValuePair<int, string> SqliteError { get; }
        
        public SqliteException(
            string message,
            KeyValuePair<int, string> sqliteError) : base(message)
        {
            SqliteError = sqliteError;
        }
    }
}
