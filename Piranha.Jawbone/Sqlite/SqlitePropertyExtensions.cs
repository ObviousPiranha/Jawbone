using System;
using System.Reflection;
using System.Text;

namespace Piranha.Jawbone.Sqlite;

static class SqlitePropertyExtensions
{
    public static StringBuilder AppendField(this StringBuilder builder, string field)
    {
        return builder.Append('`').Append(field).Append('`');
    }

    public static StringBuilder AppendProperty<T>(
        this StringBuilder builder,
        SqliteProperty<T> property)
    {
        builder
            .AppendField(property.ColumnName)
            .Append(' ')
            .Append(property.PropertyHandler.DataType);

        if (!property.CanBeNull)
            builder.Append(" NOT NULL");

        return builder;
    }

    public static StringBuilder AppendKey<T>(this StringBuilder builder, SqliteProperty<T> property)
    {
        var key = property.Key ?? throw new NullReferenceException("Key must not be null.");
        var order = key.IsDescending ? " DESC" : " ASC";
        return builder.AppendField(property.ColumnName).Append(order);
    }
}
