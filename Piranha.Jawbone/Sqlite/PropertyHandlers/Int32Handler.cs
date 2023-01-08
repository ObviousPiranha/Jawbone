namespace Piranha.Jawbone.Sqlite;

class Int32Handler : ITypeHandler<int>
{
    public string DataType => TypeHandler.Integer;

    public void BindProperty(SqliteStatement statement, int index, int value)
    {
        statement.BindInt64(index, value);
    }

    public int LoadProperty(SqliteReader reader, int index)
    {
        return reader.ColumnInt32(index);
    }
}
