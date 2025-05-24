using System;

namespace Jawbone.Sqlite;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SqliteKey : Attribute
{
    public int Ordinal { get; set; }
    public bool IsDescending { get; set; }
    public bool IsAscending
    {
        get => !IsDescending;
        set => IsDescending = !value;
    }
}
