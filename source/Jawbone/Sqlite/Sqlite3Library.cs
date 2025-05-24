using Jawbone.Generation;

namespace Jawbone.Sqlite;

// https://www.sqlite.org/c3ref/funclist.html
[MapNativeFunctions]
public sealed partial class Sqlite3Library
{
    public partial int Open(string filename, out nint database);
    public partial int OpenV2(string filename, out nint database, int flags, string? zVfs);
    public partial int Close(nint database);
    public partial int Exec(nint database, string sql, nint callback, nint argument, nint errorMessage);
    public partial int PrepareV2(nint database, string sql, int byteCount, out nint statement, nint tail);
    public partial int PrepareV3(nint database, string sql, int byteCount, uint prepFlags, out nint statement, nint tail);
    public partial int Finalize(nint statement);
    public partial nint DbHandle(nint statement);
    public partial int Reset(nint statement);
    public partial int Step(nint statement);
    public partial int ColumnCount(nint statement);
    public partial int ClearBindings(nint statement);
    public partial int BindInt64(nint statement, int index, long value);
    public partial int BindText(nint statement, int index, string data, int byteCount, nint parameter);
    public partial int BindText(nint statement, int index, in byte data, int byteCount, nint parameter);
    public partial int BindDouble(nint statement, int index, double value);
    public partial int BindBlob(nint statement, int index, in byte data, int byteCount, nint parameter);
    public partial int BindNull(nint statement, int index);
    public partial CString ColumnName(nint statement, int index);
    public partial int ColumnInt(nint statement, int index);
    public partial long ColumnInt64(nint statement, int index);
    public partial double ColumnDouble(nint statement, int index);
    public partial nint ColumnText(nint statement, int index);
    public partial nint ColumnText16(nint statement, int index);
    public partial int ColumnType(nint statement, int index);
    public partial nint ColumnBlob(nint statement, int index);
    public partial int ColumnBytes(nint statement, int index);
    public partial int ColumnBytes16(nint statement, int index);
    public partial int Errcode(nint database);
    public partial CString Errmsg(nint database);
    public partial CString Errstr(int errorCode);
    public partial int Changes(nint database);
    public partial int TotalChanges(nint database);
    public partial int Limit(nint database, int id, int newVal);
    public partial int Initialize();
    public partial int Shutdown();
    public partial CString Libversion();
}
