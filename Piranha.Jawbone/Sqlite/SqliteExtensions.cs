using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools;

namespace Piranha.Sqlite
{
    public static class SqliteExtensions
    {
        public static IServiceCollection AddSqlite3(
            this IServiceCollection services,
            string dll)
        {
            return services.AddNativeLibrary<ISqlite3>(
                _ => NativeLibraryInterface.Create<ISqlite3>(dll, ResolveName));
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

        public static KeyValuePair<int, string> GetError(this ISqlite3 sqlite3, int errorCode)
        {
            return KeyValuePair.Create(errorCode, sqlite3.Errstr(errorCode) ?? string.Empty);
        }

        public static void ThrowOnError(
            this ISqlite3 sqlite3,
            IntPtr database,
            int result)
        {
            if (result != SqliteResult.Ok)
            {
                throw new SqliteException(
                    sqlite3.Errmsg(database) ?? string.Empty,
                    sqlite3.GetError(result));
            }
        }
    }
}
