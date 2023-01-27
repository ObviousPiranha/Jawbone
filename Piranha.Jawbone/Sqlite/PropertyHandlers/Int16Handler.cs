namespace Piranha.Jawbone.Sqlite;

class Int16Handler : ITypeHandler<short>
{
    public string DataType => TypeHandler.Integer;

    public void BindProperty(SqliteStatement statement, int index, short value)
    {
        statement.BindInt64(index, value);
    }

    public short LoadProperty(SqliteReader reader, int index)
    {
        var result = unchecked((short)reader.ColumnInt64(index));
        return result;
    }
}
