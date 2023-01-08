namespace Piranha.Jawbone.Sqlite;

class UInt16Handler : ITypeHandler<ushort>
{
    public string DataType => TypeHandler.Integer;

    public void BindProperty(SqliteStatement statement, int index, ushort value)
    {
        statement.BindInt64(index, value);
    }

    public ushort LoadProperty(SqliteReader reader, int index)
    {
        return unchecked((ushort)reader.ColumnInt64(index));
    }
}
