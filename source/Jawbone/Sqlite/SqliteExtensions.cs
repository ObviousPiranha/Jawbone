using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Jawbone.Sqlite;

public static class SqliteExtensions
{
    public static KeyValuePair<int, string> GetError(this Sqlite3Library sqlite3, int errorCode)
    {
        return KeyValuePair.Create(errorCode, sqlite3.Errstr(errorCode).GetStringOrEmpty());
    }
}
