using System;
using System.Diagnostics;
using Piranha.Tools.CollectionExtensions;

namespace Piranha.Sqlite
{
    static class SqliteLimit
    {
        public const int Length = 0;
        public const int SqlLength = 1;
        public const int Column = 2;
        public const int ExprDepth = 3;
        public const int CompoundSelect = 4;
        public const int VdbeOp = 5;
        public const int FunctionArg = 6;
        public const int Attached = 7;
        public const int LikePatternLength = 8;
        public const int VariableNumber = 9;
        public const int TriggerDepth = 10;
        public const int WorkerThreads = 11;
    }

    public enum Synchronous : int
    {
        Off = 0,
        Normal = 1,
        Full = 2,
        Extra = 3
    }

    public enum JournalMode : int
    {
        Delete = 0,
        Truncate = 1,
        Persist = 2,
        Memory = 3,
        WriteAheadLog = 4,
        Off = 5
    }

    // https://www.sqlite.org/c3ref/open.html
    [Flags] enum SqliteOpen : int
    {
        ReadOnly = 1 << 0,
        ReadWrite = 1 << 1,
        Create = 1 << 2,
        Uri = 1 << 6,
        Memory = 1 << 7,
        NoMutex = 1 << 15,
        FullMutex = 1 << 16,
        SharedCache = 1 << 17,
        PrivateCache = 1 << 18
    }

    public enum TempStore
    {
        Default = 0,
        File = 1,
        Memory = 2
    }

    public class SqliteDatabase : IDisposable
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