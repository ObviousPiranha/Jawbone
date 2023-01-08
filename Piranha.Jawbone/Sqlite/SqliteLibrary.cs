using System;
using System.Collections.Generic;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sqlite;

public sealed class SqliteLibrary : IDisposable
{
    private readonly NativeLibraryInterface<ISqlite3> _nativeLibraryInterface;

    public ISqlite3 Library => _nativeLibraryInterface.Library;

    public SqliteLibrary(string file)
    {
        _nativeLibraryInterface = NativeLibraryInterface.FromFile<ISqlite3>(
            file,
            methodName => NativeLibraryInterface.PascalCaseToSnakeCase("sqlite3", methodName));

        try
        {
            // https://www.sqlite.org/c3ref/initialize.html
            var result = Library.Initialize();

            if (result != SqliteResult.Ok)
            {
                throw new SqliteException(
                    "Error on sqlite3_initialize().",
                    KeyValuePair.Create(result, result.ToString()));
            }
        }
        catch
        {
            _nativeLibraryInterface.Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        _nativeLibraryInterface.Library.Shutdown();
        _nativeLibraryInterface.Dispose();
    }
}
