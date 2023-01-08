namespace Piranha.Jawbone.Sqlite;

class StringHandler : ITypeHandler<string>
{
    public string DataType => TypeHandler.Text;
    
    public void BindProperty(SqliteStatement statement, int index, string value)
    {
        statement.BindText(index, value);
    }

    public string LoadProperty(SqliteReader reader, int index)
    {
        var span = reader.ColumnUtf16(index);
        return span.IsEmpty ? "" : new string(span);
    }
}
