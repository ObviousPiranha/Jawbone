namespace Jawbone.Sqlite;

class NullableHandler<T> : ITypeHandler<T?> where T : struct
{
    private readonly ITypeHandler<T> _handler;

    public string DataType => _handler.DataType;

    public NullableHandler(ITypeHandler<T> handler)
    {
        _handler = handler;
    }

    public void BindProperty(SqliteStatement statement, int index, T? value)
    {
        if (value.HasValue)
            _handler.BindProperty(statement, index, value.Value);
        else
            statement.BindNull(index);
    }

    public T? LoadProperty(SqliteReader reader, int index)
    {
        if (reader.IsNull(index))
            return default;
        else
            return _handler.LoadProperty(reader, index);
    }
}
