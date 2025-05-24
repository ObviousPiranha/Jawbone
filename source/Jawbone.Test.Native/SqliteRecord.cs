using Jawbone.Sqlite;
using System;

namespace Jawbone.Test.Native;

public record SqliteRecord
{
    [SqliteKey]
    public Guid Id { get; init; }
    public string? Name { get; init; }
}
