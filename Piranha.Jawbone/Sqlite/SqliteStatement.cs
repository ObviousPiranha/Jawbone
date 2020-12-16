using System;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.Sqlite
{
    public static class SqliteStatementExtensions
    {

    }

    public class SqliteStatement : IDisposable
    {
        private static readonly IntPtr Transient = new IntPtr(-1);
        private static readonly IntPtr Static = new IntPtr(0);

        private readonly IntPtr _database;
        private readonly ISqlite3 _sqlite3;
        private IntPtr _statement;

        public string Sql { get; }

        private void ThrowOnError(int result) => _sqlite3.ThrowOnError(_database, result);

        public SqliteStatement(ISqlite3 sqlite3, IntPtr database, string sql)
        {
            _database = database;
            _sqlite3 = sqlite3;
            Sql = sql;
            
            var result = _sqlite3.PrepareV2(
                _database,
                sql,
                -1,
                out _statement,
                IntPtr.Zero);
            
            try
            {
                ThrowOnError(result);
            }
            catch
            {
                if (_statement.IsValid())
                    _sqlite3.Finalize(_statement);

                throw;
            }
        }

        public void Dispose()
        {
            _sqlite3.Finalize(_statement);
            _statement = IntPtr.Zero;
        }

        public string? ColumnName(int index) => _sqlite3.ColumnName(_statement, index);

        public void BindInt64(int index, long value)
        {
            var result = _sqlite3.BindInt64(_statement, index, value);
            ThrowOnError(result);
        }

        public void BindText(int index, string value)
        {
            int result;

            if (value is null)
            {
                result = _sqlite3.BindNull(_statement, index);
            }
            else
            {
                result = _sqlite3.BindText(
                    _statement,
                    index,
                    value,
                    -1,
                    Transient);
            }

            ThrowOnError(result);
        }

        public void BindFloat64(int index, double value)
        {
            var result = _sqlite3.BindDouble(_statement, index, value);
            ThrowOnError(result);
        }

        public void BindBlob(int index, ReadOnlySpan<byte> value)
        {
            int result;

            if (value.IsEmpty)
            {
                result = _sqlite3.BindNull(_statement, index);
            }
            else
            {
                result = _sqlite3.BindBlob(
                    _statement,
                    index,
                    value[0],
                    value.Length,
                    Transient);
            }

            ThrowOnError(result);
        }

        public void BindNull(int index)
        {
            var result = _sqlite3.BindNull(_statement, index);
            ThrowOnError(result);
        }

        public void ClearBindings()
        {
            var result = _sqlite3.ClearBindings(_statement);
            ThrowOnError(result);
        }

        public int Execute()
        {
            var result = _sqlite3.Step(_statement);

            if (result == SqliteResult.RowReady)
            {
                var resetResult = _sqlite3.Reset(_statement);
                ThrowOnError(resetResult);
                return 0;
            }
            else if (result == SqliteResult.Ok || result == SqliteResult.Done)
            {
                var changes = _sqlite3.Changes(_database);
                var resetResult = _sqlite3.Reset(_statement);
                ThrowOnError(resetResult);
                return changes;
            }
            else
            {
                throw new SqliteException(
                    _sqlite3.Errmsg(_database) ?? string.Empty,
                    _sqlite3.GetError(result));
            }
        }

        public SqliteReader GetReader() => new SqliteReader(_sqlite3, _database, _statement, false);

        public override string ToString() => Sql;
    }
}
