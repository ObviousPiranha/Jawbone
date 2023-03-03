namespace Piranha.Jawbone.Sqlite;

class ByteArrayHandler : ITypeHandler<byte[]>
{
    public string DataType => TypeHandler.Blob;

    public void BindProperty(SqliteStatement statement, int index, byte[] value)
    {
        statement.BindBlob(index, value);
    }

    public byte[] LoadProperty(SqliteReader reader, int index)
    {
        return reader.ColumnBlob(index).ToArray();
    }
}
