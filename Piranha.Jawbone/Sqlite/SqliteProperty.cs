using System;
using System.Reflection;
using System.Text;

namespace Piranha.Jawbone.Sqlite;

readonly struct SqliteProperty<T>
{
    public static int CompareKeys(SqliteProperty<T> a, SqliteProperty<T> b)
    {
        var ka = a.Key ?? throw new NullReferenceException();
        var kb = b.Key ?? throw new NullReferenceException();
        return ka.Ordinal.CompareTo(kb.Ordinal);
    }

    public string ColumnName { get; }
    public PropertyInfo Info { get; }
    public SqliteKey? Key { get; }
    public bool CanBeNull { get; }
    public IPropertyHandler<T> PropertyHandler { get; }

    public SqliteProperty(
        PropertyInfo propertyInfo,
        IPropertyHandler<T> propertyHandler,
        Func<string, string> namingPolicy)
    {
        Info = propertyInfo;
        Key = propertyInfo.GetCustomAttribute<SqliteKey>();
        PropertyHandler = propertyHandler;

        var column = propertyInfo.GetCustomAttribute<SqliteColumn>();
        ColumnName = column is null ? namingPolicy.Invoke(Info.Name) : column.Name;

        var notNull = propertyInfo.GetCustomAttribute<SqliteNotNull>();
        var t = Info.PropertyType;
        CanBeNull = t.IsValueType ? t.IsNullableValueType() : notNull is null;
    }
}
