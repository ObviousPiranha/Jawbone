namespace Piranha.Jawbone.Sqlite;

class Int8Handler : ITypeHandler<sbyte>
{
    public string DataType => TypeHandler.Integer;
    
    public void BindProperty(SqliteStatement statement, int index, sbyte value)
    {
        statement.BindInt64(index, value);
    }

    public sbyte LoadProperty(SqliteReader reader, int index)
    {
        var result = unchecked((sbyte)reader.ColumnInt64(index));
        return result;
    }
}
