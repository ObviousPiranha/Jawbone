namespace Jawbone.Sqlite;

class Int64Handler : ITypeHandler<long>
{
    public string DataType => TypeHandler.Integer;

    public void BindProperty(SqliteStatement statement, int index, long value)
    {
        statement.BindInt64(index, value);
    }

    public long LoadProperty(SqliteReader reader, int index)
    {
        return reader.ColumnInt64(index);
    }
}
