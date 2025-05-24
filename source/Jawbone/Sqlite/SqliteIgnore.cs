using System;

namespace Jawbone.Sqlite;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SqliteIgnore : Attribute
{
}
