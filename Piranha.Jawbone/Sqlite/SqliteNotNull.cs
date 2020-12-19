using System;

namespace Piranha.Jawbone.Sqlite
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SqliteNotNull : Attribute
    {
    }
}
