using System;
using System.Collections.Generic;

namespace Jawbone.Sqlite;

public class SqliteException : Exception
{
    public KeyValuePair<int, string> SqliteError { get; }

    public SqliteException(
        string message,
        KeyValuePair<int, string> sqliteError) : base(message)
    {
        SqliteError = sqliteError;
    }

    public static void ThrowOnError(
        Sqlite3Library sqlite3,
        nint database,
        int result)
    {
        if (result != SqliteResult.Ok &&
            result != SqliteResult.RowReady &&
            result != SqliteResult.Done)
        {
            throw new SqliteException(
                sqlite3.Errmsg(database).GetStringOrEmpty(),
                sqlite3.GetError(result));
        }
    }
}
