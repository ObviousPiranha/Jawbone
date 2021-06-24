using System;
using System.Collections.Generic;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sqlite
{
    public sealed class SqliteLibrary : IDisposable
    {
        private readonly NativeLibraryInterface<ISqlite3> _nativeLibraryInterface;

        public ISqlite3 Library => _nativeLibraryInterface.Library;

        public SqliteLibrary(string file)
        {
            _nativeLibraryInterface = NativeLibraryInterface.FromFile<ISqlite3>(file, ResolveName);

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

        public static string ResolveName(string methodName)
        {
            var prefix = "sqlite3";
            var chars = new char[prefix.Length + methodName.Length * 2];
            prefix.AsSpan().CopyTo(chars);

            int n = prefix.Length;

            for (int i = 0; i < methodName.Length; ++i)
            {
                char c = methodName[i];

                if (char.IsUpper(c))
                {
                    chars[n++] = '_';
                    chars[n++] = char.ToLowerInvariant(c);
                }
                else
                {
                    chars[n++] = c;
                }
            }

            return new string(chars, 0, n);
        }
    }
}