using Jawbone.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace Jawbone.Test.Native;

[Collection(JawboneServiceCollection.Name)]
public class SqliteTest
{
    private const string DatabasePath = ":memory:";
    private static readonly SqliteTable<SqliteRecord> Table = new();
    private readonly Sqlite3Library _sqlite3;

    public SqliteTest(ServiceFixture fixture)
    {
        _sqlite3 = fixture.ServiceProvider.GetRequiredService<Sqlite3Library>();
    }

    [Fact]
    public void HasVersion()
    {
        var version = _sqlite3.Libversion();
        Assert.NotEqual(default, version.Address);
    }

    [Fact]
    public void DoTheThing()
    {
        var records = new SqliteRecord[]
        {
            new SqliteRecord { Id = Guid.NewGuid(), Name = "One" },
            new SqliteRecord { Id = Guid.NewGuid(), Name = "Two" },
            new SqliteRecord { Id = Guid.NewGuid(), Name = "Three" }
        };

        using (var database = SqliteDatabase.Create(_sqlite3, DatabasePath))
        {
            Table.CreateTable(database);
            Table.Insert(database, records);

            var storedRecords = Table.Values(database).ToArray();
            Assert.False(object.ReferenceEquals(records, storedRecords));
            Assert.Equal(records, storedRecords);
        }
    }
}
