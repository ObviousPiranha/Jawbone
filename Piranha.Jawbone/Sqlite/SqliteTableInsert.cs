using System;

namespace Piranha.Sqlite
{
    public class SqliteTableInsert<T> : IDisposable where T : class
    {
        private readonly SqliteTable<T> _table;
        private readonly SqliteStatement _statement;

        public SqliteTableInsert(
            SqliteTable<T> table,
            SqliteDatabase database,
            ConflictResolution conflictResolution = ConflictResolution.Default)
        {
            _table = table;

            var sql = table.InsertSql(1, conflictResolution);
            _statement = database.Prepare(sql);
        }

        public void Dispose()
        {
            _statement.Dispose();
        }

        public void Insert(T record)
        {
            _table.Bind(_statement, 1, record);
            _statement.Execute();
        }
    }
}