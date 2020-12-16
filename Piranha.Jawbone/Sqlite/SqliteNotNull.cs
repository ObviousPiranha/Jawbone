using System;

namespace Piranha.Sqlite
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SqliteNotNull : Attribute
    {
    }
}