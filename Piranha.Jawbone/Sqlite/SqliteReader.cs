using Piranha.Jawbone.Extensions;
using System;

namespace Piranha.Jawbone.Sqlite;

public sealed class SqliteReader : IDisposable
{
    private readonly ISqlite3 _sqlite3;
    private readonly IntPtr _database;
    private readonly IntPtr _statement;
    private readonly bool _ownsStatement;

    public int ColumnCount => _sqlite3.ColumnCount(_statement);

    public SqliteReader(
        ISqlite3 sqlite3,
        IntPtr database,
        IntPtr statement,
        bool ownsStatement)
    {
        _sqlite3 = sqlite3;
        _database = database;
        _statement = statement;
        _ownsStatement = ownsStatement;
    }

    public void Dispose()
    {
        var result = _ownsStatement ?
            _sqlite3.Finalize(_statement) :
            _sqlite3.Reset(_statement);

        _sqlite3.ThrowOnError(_database, result);
    }

    private void ThrowOnError()
    {
        var errorCode = _sqlite3.Errcode(_database);
        _sqlite3.ThrowOnError(_database, errorCode);
    }

    public string? ColumnName(int index) => _sqlite3.ColumnName(_statement, index);

    public string?[] GetColumnNames()
    {
        int n = ColumnCount;

        if (n < 1)
        {
            return Array.Empty<string>();
        }
        else
        {
            var result = new string?[n];

            for (int i = 0; i < n; ++i)
                result[i] = ColumnName(i);

            return result;
        }
    }

    public bool TryRead()
    {
        var result = _sqlite3.Step(_statement);
        return result == SqliteResult.RowReady;
    }

    public bool IsNull(int index)
    {
        return _sqlite3.ColumnType(_statement, index) == SqliteType.Null;
    }

    public int ColumnInt32(int index)
    {
        return _sqlite3.ColumnInt(_statement, index);
    }

    public long ColumnInt64(int index)
    {
        return _sqlite3.ColumnInt64(_statement, index);
    }

    public double ColumnDouble(int index)
    {
        return _sqlite3.ColumnDouble(_statement, index);
    }

    public ReadOnlySpan<char> ColumnUtf16(int index)
    {
        var pointer = _sqlite3.ColumnText16(_statement, index);
        ThrowOnError();
        var byteCount = _sqlite3.ColumnBytes16(_statement, index);

        if (0 < byteCount && pointer.IsValid())
            unsafe
            {
                return new ReadOnlySpan<char>(
                    pointer.ToPointer(),
                    byteCount / 2);
            }

        return default;
    }

    public ReadOnlySpan<byte> ColumnUtf8(int index)
    {
        var pointer = _sqlite3.ColumnText(_statement, index);
        ThrowOnError();
        var byteCount = _sqlite3.ColumnBytes(_statement, index);

        if (0 < byteCount && pointer.IsValid())
            unsafe
            {
                return new ReadOnlySpan<byte>(
                    pointer.ToPointer(),
                    byteCount);
            }

        return default;
    }

    public ReadOnlySpan<byte> ColumnBlob(int index)
    {
        // https://www.sqlite.org/c3ref/column_blob.html
        // "The safest policy is to invoke these routines in one of the following ways:"
        // "sqlite3_column_blob() followed by sqlite3_column_bytes()"

        var pointer = _sqlite3.ColumnBlob(_statement, index);
        ThrowOnError();
        var byteCount = _sqlite3.ColumnBytes(_statement, index);

        if (0 < byteCount && pointer.IsValid())
            unsafe
            {
                return new ReadOnlySpan<byte>(
                    pointer.ToPointer(),
                    byteCount);
            }

        return default;
    }
}
