namespace Piranha.Jawbone.Sqlite;

class Float64Handler : ITypeHandler<double>
{
    public string DataType => TypeHandler.Real;

    public void BindProperty(SqliteStatement statement, int index, double value)
    {
        statement.BindFloat64(index, value);
    }

    public double LoadProperty(SqliteReader reader, int index)
    {
        return reader.ColumnDouble(index);
    }
}
