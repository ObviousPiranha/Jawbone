using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sqlite
{
    public static class SqliteExtensions
    {
        public static IServiceCollection AddSqlite3(this IServiceCollection services)
        {
            return services
                .AddSingleton<SqliteLibrary>(
                    _ => new SqliteLibrary("PiranhaNative.dll"))
                .AddSingleton<ISqlite3>(
                    serviceProvider => serviceProvider.GetRequiredService<SqliteLibrary>().Library);
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
