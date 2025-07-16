// Code generated at 2025-06-21T19:13:07.

namespace Jawbone.Sqlite;

public sealed unsafe class Sqlite3Library
{
    private readonly nint _fp_BindBlob;
    private readonly nint _fp_BindDouble;
    private readonly nint _fp_BindInt64;
    private readonly nint _fp_BindNull;
    private readonly nint _fp_BindText;
    private readonly nint _fp_Changes;
    private readonly nint _fp_ClearBindings;
    private readonly nint _fp_Close;
    private readonly nint _fp_ColumnBlob;
    private readonly nint _fp_ColumnBytes;
    private readonly nint _fp_ColumnBytes16;
    private readonly nint _fp_ColumnCount;
    private readonly nint _fp_ColumnDouble;
    private readonly nint _fp_ColumnInt;
    private readonly nint _fp_ColumnInt64;
    private readonly nint _fp_ColumnName;
    private readonly nint _fp_ColumnText;
    private readonly nint _fp_ColumnText16;
    private readonly nint _fp_ColumnType;
    private readonly nint _fp_DbHandle;
    private readonly nint _fp_Errcode;
    private readonly nint _fp_Errmsg;
    private readonly nint _fp_Errstr;
    private readonly nint _fp_Exec;
    private readonly nint _fp_Finalize;
    private readonly nint _fp_Initialize;
    private readonly nint _fp_Libversion;
    private readonly nint _fp_Limit;
    private readonly nint _fp_Open;
    private readonly nint _fp_OpenV2;
    private readonly nint _fp_PrepareV2;
    private readonly nint _fp_PrepareV3;
    private readonly nint _fp_Reset;
    private readonly nint _fp_Shutdown;
    private readonly nint _fp_Step;
    private readonly nint _fp_TotalChanges;

    public Sqlite3Library(
        System.Func<string, nint> loader)
    {
        _fp_BindBlob = loader.Invoke(nameof(BindBlob));
        _fp_BindDouble = loader.Invoke(nameof(BindDouble));
        _fp_BindInt64 = loader.Invoke(nameof(BindInt64));
        _fp_BindNull = loader.Invoke(nameof(BindNull));
        _fp_BindText = loader.Invoke(nameof(BindText));
        _fp_Changes = loader.Invoke(nameof(Changes));
        _fp_ClearBindings = loader.Invoke(nameof(ClearBindings));
        _fp_Close = loader.Invoke(nameof(Close));
        _fp_ColumnBlob = loader.Invoke(nameof(ColumnBlob));
        _fp_ColumnBytes = loader.Invoke(nameof(ColumnBytes));
        _fp_ColumnBytes16 = loader.Invoke(nameof(ColumnBytes16));
        _fp_ColumnCount = loader.Invoke(nameof(ColumnCount));
        _fp_ColumnDouble = loader.Invoke(nameof(ColumnDouble));
        _fp_ColumnInt = loader.Invoke(nameof(ColumnInt));
        _fp_ColumnInt64 = loader.Invoke(nameof(ColumnInt64));
        _fp_ColumnName = loader.Invoke(nameof(ColumnName));
        _fp_ColumnText = loader.Invoke(nameof(ColumnText));
        _fp_ColumnText16 = loader.Invoke(nameof(ColumnText16));
        _fp_ColumnType = loader.Invoke(nameof(ColumnType));
        _fp_DbHandle = loader.Invoke(nameof(DbHandle));
        _fp_Errcode = loader.Invoke(nameof(Errcode));
        _fp_Errmsg = loader.Invoke(nameof(Errmsg));
        _fp_Errstr = loader.Invoke(nameof(Errstr));
        _fp_Exec = loader.Invoke(nameof(Exec));
        _fp_Finalize = loader.Invoke(nameof(Finalize));
        _fp_Initialize = loader.Invoke(nameof(Initialize));
        _fp_Libversion = loader.Invoke(nameof(Libversion));
        _fp_Limit = loader.Invoke(nameof(Limit));
        _fp_Open = loader.Invoke(nameof(Open));
        _fp_OpenV2 = loader.Invoke(nameof(OpenV2));
        _fp_PrepareV2 = loader.Invoke(nameof(PrepareV2));
        _fp_PrepareV3 = loader.Invoke(nameof(PrepareV3));
        _fp_Reset = loader.Invoke(nameof(Reset));
        _fp_Shutdown = loader.Invoke(nameof(Shutdown));
        _fp_Step = loader.Invoke(nameof(Step));
        _fp_TotalChanges = loader.Invoke(nameof(TotalChanges));
    }

    public int BindBlob(
        nint statement,
        int index,
        in byte data,
        int byteCount,
        nint parameter)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, void*, int, nint, int
            >)_fp_BindBlob;
        fixed (void* __p_data = &data)
        {
            var __result = __fp(statement, index, __p_data, byteCount, parameter);
            return __result;
        }
    }

    public int BindDouble(
        nint statement,
        int index,
        double value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, double, int
            >)_fp_BindDouble;
        var __result = __fp(statement, index, value);
        return __result;
    }

    public int BindInt64(
        nint statement,
        int index,
        long value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, long, int
            >)_fp_BindInt64;
        var __result = __fp(statement, index, value);
        return __result;
    }

    public int BindNull(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int
            >)_fp_BindNull;
        var __result = __fp(statement, index);
        return __result;
    }

    public int BindText(
        nint statement,
        int index,
        string data,
        int byteCount,
        nint parameter)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, string, int, nint, int
            >)_fp_BindText;
        var __result = __fp(statement, index, data, byteCount, parameter);
        return __result;
    }

    public int BindText(
        nint statement,
        int index,
        in byte data,
        int byteCount,
        nint parameter)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, void*, int, nint, int
            >)_fp_BindText;
        fixed (void* __p_data = &data)
        {
            var __result = __fp(statement, index, __p_data, byteCount, parameter);
            return __result;
        }
    }

    public int Changes(
        nint database)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_Changes;
        var __result = __fp(database);
        return __result;
    }

    public int ClearBindings(
        nint statement)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_ClearBindings;
        var __result = __fp(statement);
        return __result;
    }

    public int Close(
        nint database)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_Close;
        var __result = __fp(database);
        return __result;
    }

    public nint ColumnBlob(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, nint
            >)_fp_ColumnBlob;
        var __result = __fp(statement, index);
        return __result;
    }

    public int ColumnBytes(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int
            >)_fp_ColumnBytes;
        var __result = __fp(statement, index);
        return __result;
    }

    public int ColumnBytes16(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int
            >)_fp_ColumnBytes16;
        var __result = __fp(statement, index);
        return __result;
    }

    public int ColumnCount(
        nint statement)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_ColumnCount;
        var __result = __fp(statement);
        return __result;
    }

    public double ColumnDouble(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, double
            >)_fp_ColumnDouble;
        var __result = __fp(statement, index);
        return __result;
    }

    public int ColumnInt(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int
            >)_fp_ColumnInt;
        var __result = __fp(statement, index);
        return __result;
    }

    public long ColumnInt64(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, long
            >)_fp_ColumnInt64;
        var __result = __fp(statement, index);
        return __result;
    }

    public CString ColumnName(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, CString
            >)_fp_ColumnName;
        var __result = __fp(statement, index);
        return __result;
    }

    public nint ColumnText(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, nint
            >)_fp_ColumnText;
        var __result = __fp(statement, index);
        return __result;
    }

    public nint ColumnText16(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, nint
            >)_fp_ColumnText16;
        var __result = __fp(statement, index);
        return __result;
    }

    public int ColumnType(
        nint statement,
        int index)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int
            >)_fp_ColumnType;
        var __result = __fp(statement, index);
        return __result;
    }

    public nint DbHandle(
        nint statement)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, nint
            >)_fp_DbHandle;
        var __result = __fp(statement);
        return __result;
    }

    public int Errcode(
        nint database)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_Errcode;
        var __result = __fp(database);
        return __result;
    }

    public CString Errmsg(
        nint database)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, CString
            >)_fp_Errmsg;
        var __result = __fp(database);
        return __result;
    }

    public CString Errstr(
        int errorCode)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, CString
            >)_fp_Errstr;
        var __result = __fp(errorCode);
        return __result;
    }

    public int Exec(
        nint database,
        string sql,
        nint callback,
        nint argument,
        nint errorMessage)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, string, nint, nint, nint, int
            >)_fp_Exec;
        var __result = __fp(database, sql, callback, argument, errorMessage);
        return __result;
    }

    public int Finalize(
        nint statement)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_Finalize;
        var __result = __fp(statement);
        return __result;
    }

    public int Initialize()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int
            >)_fp_Initialize;
        var __result = __fp();
        return __result;
    }

    public CString Libversion()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            CString
            >)_fp_Libversion;
        var __result = __fp();
        return __result;
    }

    public int Limit(
        nint database,
        int id,
        int newVal)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int, int
            >)_fp_Limit;
        var __result = __fp(database, id, newVal);
        return __result;
    }

    public int Open(
        string filename,
        out nint database)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, void*, int
            >)_fp_Open;
        fixed (void* __p_database = &database)
        {
            var __result = __fp(filename, __p_database);
            return __result;
        }
    }

    public int OpenV2(
        string filename,
        out nint database,
        int flags,
        string? zVfs)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            string, void*, int, string?, int
            >)_fp_OpenV2;
        fixed (void* __p_database = &database)
        {
            var __result = __fp(filename, __p_database, flags, zVfs);
            return __result;
        }
    }

    public int PrepareV2(
        nint database,
        string sql,
        int byteCount,
        out nint statement,
        nint tail)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, string, int, void*, nint, int
            >)_fp_PrepareV2;
        fixed (void* __p_statement = &statement)
        {
            var __result = __fp(database, sql, byteCount, __p_statement, tail);
            return __result;
        }
    }

    public int PrepareV3(
        nint database,
        string sql,
        int byteCount,
        uint prepFlags,
        out nint statement,
        nint tail)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, string, int, uint, void*, nint, int
            >)_fp_PrepareV3;
        fixed (void* __p_statement = &statement)
        {
            var __result = __fp(database, sql, byteCount, prepFlags, __p_statement, tail);
            return __result;
        }
    }

    public int Reset(
        nint statement)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_Reset;
        var __result = __fp(statement);
        return __result;
    }

    public int Shutdown()
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int
            >)_fp_Shutdown;
        var __result = __fp();
        return __result;
    }

    public int Step(
        nint statement)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_Step;
        var __result = __fp(statement);
        return __result;
    }

    public int TotalChanges(
        nint database)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int
            >)_fp_TotalChanges;
        var __result = __fp(database);
        return __result;
    }
}
