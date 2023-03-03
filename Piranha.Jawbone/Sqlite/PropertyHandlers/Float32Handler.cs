namespace Piranha.Jawbone.Sqlite;

class Float32Handler : ITypeHandler<float>
{
    public string DataType => TypeHandler.Real;

    public void BindProperty(SqliteStatement statement, int index, float value)
    {
        statement.BindFloat64(index, value);
    }

    public float LoadProperty(SqliteReader reader, int index)
    {
        return (float)reader.ColumnDouble(index);
    }
}
