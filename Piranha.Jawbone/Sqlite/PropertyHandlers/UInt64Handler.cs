namespace Piranha.Jawbone.Sqlite;

class UInt64Handler : ITypeHandler<ulong>
{
    public string DataType => TypeHandler.Integer;

    public void BindProperty(SqliteStatement statement, int index, ulong value)
    {
        var n = unchecked((long)value);
        statement.BindInt64(index, n);
    }

    public ulong LoadProperty(SqliteReader reader, int index)
    {
        var n = unchecked((ulong)reader.ColumnInt64(index));
        return n;
    }
}
