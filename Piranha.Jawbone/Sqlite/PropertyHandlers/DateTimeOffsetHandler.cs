using System;

namespace Piranha.Jawbone.Sqlite;

class DateTimeOffsetHandler : ITypeHandler<DateTimeOffset>
{
    public string DataType => TypeHandler.Text;

    public void BindProperty(SqliteStatement statement, int index, DateTimeOffset value)
    {
        statement.BindText(index, value.ToString("u"));
    }

    public DateTimeOffset LoadProperty(SqliteReader reader, int index)
    {
        var text = reader.ColumnUtf16(index);
        DateTimeOffset.TryParse(text, out var result);
        return result;
    }
}
