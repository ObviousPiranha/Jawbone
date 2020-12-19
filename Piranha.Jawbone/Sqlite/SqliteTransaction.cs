using System;

namespace Piranha.Jawbone.Sqlite
{
    public class SqliteTransaction : IDisposable
    {
        private readonly SqliteDatabase _database;
        private bool _wasCommitted = false;

        public SqliteTransaction(SqliteDatabase database)
        {
            _database = database;
            _database.Execute("BEGIN TRANSACTION");
        }

        public void Commit()
        {
            if (!_wasCommitted)
            {
                _database.Execute("COMMIT");
                _wasCommitted = true;
            }
        }

        public void Dispose()
        {
            if (!_wasCommitted)
            {
                _database.Execute("ROLLBACK");
            }
        }
    }
}
