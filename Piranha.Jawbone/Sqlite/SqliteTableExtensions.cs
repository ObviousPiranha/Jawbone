using System.Collections.Generic;

namespace Piranha.Jawbone.Sqlite;

public static class SqliteTableExtensions
{
    public static IEnumerable<T> Values<T>(this SqliteTable<T> table, SqliteDatabase database) where T : class, new()
    {
        return table.Values(database, () => new T());
    }

    public static IEnumerable<KeyValuePair<long, T>> ValuesWithRowId<T>(this SqliteTable<T> table, SqliteDatabase database) where T : class, new()
    {
        return table.ValuesWithRowId(database, () => new T());
    }
}
