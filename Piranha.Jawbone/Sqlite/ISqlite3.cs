using System;

namespace Piranha.Jawbone.Sqlite
{
    // https://www.sqlite.org/c3ref/funclist.html

    public interface ISqlite3
    {
        int Open(string filename, out IntPtr database);
        int OpenV2(string filename, out IntPtr database, int flags, byte[]? zVfs);
        int Close(IntPtr database);
        int Exec(IntPtr database, string sql, IntPtr callback, IntPtr argument, IntPtr errorMessage);
        int PrepareV2(IntPtr database, string sql, int byteCount, out IntPtr statement, IntPtr tail);
        int PrepareV3(IntPtr database, string sql, int byteCount, uint prepFlags, out IntPtr statement, IntPtr tail);
        int Finalize(IntPtr statement);
        IntPtr DbHandle(IntPtr statement);
        int Reset(IntPtr statement);
        int Step(IntPtr statement);
        Int32 ColumnCount(IntPtr statement);
        int ClearBindings(IntPtr statement);
        int BindInt64(IntPtr statement, int index, Int64 value);
        int BindText(IntPtr statement, int index, string data, int byteCount, IntPtr parameter);
        int BindText(IntPtr statement, int index, in byte data, int byteCount, IntPtr parameter);
        int BindDouble(IntPtr statement, int index, double value);
        int BindBlob(IntPtr statement, int index, in byte data, int byteCount, IntPtr parameter);
        int BindNull(IntPtr statement, int index);
        string? ColumnName(IntPtr statement, int index);
        Int32 ColumnInt(IntPtr statement, int index);
        Int64 ColumnInt64(IntPtr statement, int index);
        double ColumnDouble(IntPtr statement, int index);
        string? ColumnText(IntPtr statement, int index);
        int ColumnType(IntPtr statement, int index);
        IntPtr ColumnBlob(IntPtr statement, int index);
        Int32 ColumnBytes(IntPtr statement, int index);
        string? Errmsg(IntPtr database);
        string? Errstr(int errorCode);
        int Changes(IntPtr database);
        int TotalChanges(IntPtr database);
        int Limit(IntPtr database, int id, int newVal);
        int Initialize();
        int Shutdown();
        string? Libversion();
    }
}
