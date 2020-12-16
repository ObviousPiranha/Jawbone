namespace Piranha.Sqlite
{
    class UInt32Handler : ITypeHandler<uint>
    {
        public string DataType => TypeHandler.Integer;

        public void BindProperty(SqliteStatement statement, int index, uint value)
        {
            statement.BindInt64(index, value);
        }

        public uint LoadProperty(SqliteReader reader, int index)
        {
            return unchecked((uint)reader.ColumnInt64(index));
        }
    }
}