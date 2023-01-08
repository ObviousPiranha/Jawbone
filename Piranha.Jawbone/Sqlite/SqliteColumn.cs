using System;

namespace Piranha.Jawbone.Sqlite;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SqliteColumn : Attribute
{
    public string Name { get; }

    public SqliteColumn(string name)
    {
        Name = name;
    }
}
