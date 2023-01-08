namespace Piranha.Jawbone.Sqlite;

class UInt8Handler : ITypeHandler<byte>
{
    public string DataType => TypeHandler.Integer;

    public void BindProperty(SqliteStatement statement, int index, byte value)
    {
        statement.BindInt64(index, value);
    }

    public byte LoadProperty(SqliteReader reader, int index)
    {
        return unchecked((byte)reader.ColumnInt64(index));
    }
}
