using System;
using Piranha.Jawbone.Sqlite;

namespace Piranha.Jawbone.Test.Native;

public record SqliteRecord
{
    [SqliteKey]
    public Guid Id { get; init; }
    public string? Name { get; init; }
}
