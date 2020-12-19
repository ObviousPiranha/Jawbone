using System;
using System.Diagnostics;
using Piranha.Jawbone.Tools.CollectionExtensions;

namespace Piranha.Jawbone.Sqlite
{
    public sealed class SqliteDatabase : IDisposable
    {
        public static SqliteDatabase Create(ISqlite3 sqlite3, string path) => new SqliteDatabase(sqlite3, path, SqliteOpen.ReadWrite | SqliteOpen.Create);
        public static SqliteDatabase Open(ISqlite3 sqlite3, string path) => new SqliteDatabase(sqlite3, path, SqliteOpen.ReadWrite);
        public static SqliteDatabase OpenRead(ISqlite3 sqlite3, string path) => new SqliteDatabase(sqlite3, path, SqliteOpen.ReadOnly);

        private readonly ISqlite3 _sqlite3;
        private IntPtr _database;

        public string Path { get; }
        public bool IsOpen => _database != IntPtr.Zero;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int VariableLimit
        {
            get
            {
                ThrowIfClosed();
                return _sqlite3.Limit(_database, SqliteLimit.VariableNumber, -1);
            }

            set
            {
                ThrowIfClosed();
                _sqlite3.Limit(_database, SqliteLimit.VariableNumber, value);
            }
        }

        internal void ThrowIfClosed()
        {
            if (!IsOpen)
                throw new ObjectDisposedException("SQLite database is not open.");
        }

        private SqliteDatabase(ISqlite3 sqlite3, string path, SqliteOpen flags)
        {
            _sqlite3 = sqlite3;
            var result = _sqlite3.OpenV2(path, out _database, (int)flags, null);

            if (result != SqliteResult.Ok)
            {
                var message = $"Unable to open " + path + ".";

                if (_database.IsValid())
                {
                    var errorMessage = _sqlite3.Errmsg(_database);
                    _sqlite3.Close(_database);
                }

                throw new SqliteException(message, sqlite3.GetError(result));
            }

            Path = path;
        }

        public void Dispose()
        {
            var result = _sqlite3.Close(_database);
            _database = IntPtr.Zero;
        }

        public void Execute(string sql)
        {
            ThrowIfClosed();

            var result = _sqlite3.Exec(
                _database,
                sql,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);
            
            _sqlite3.ThrowOnError(_database, result);
        }

        public SqliteStatement Prepare(string sql)
        {
            ThrowIfClosed();
            return new SqliteStatement(_sqlite3, _database, sql);
        }

        public SqliteReader Read(string sql)
        {
            ThrowIfClosed();
            var result = _sqlite3.PrepareV2(
                _database,
                sql,
                -1,
                out var statement,
                IntPtr.Zero);
            
            try
            {
                _sqlite3.ThrowOnError(_database, result);
            }
            catch
            {
                if (statement.IsValid())
                    _sqlite3.Finalize(statement);
                
                throw;
            }
            
            return new SqliteReader(_sqlite3, _database, statement, true);
        }

        public override string ToString() => Path;
    }
}
