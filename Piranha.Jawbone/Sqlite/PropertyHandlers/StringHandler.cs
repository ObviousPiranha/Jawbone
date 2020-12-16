namespace Piranha.Sqlite
{
    class StringHandler : ITypeHandler<string>
    {
        public string DataType => TypeHandler.Text;
        
        public void BindProperty(SqliteStatement statement, int index, string value)
        {
            statement.BindText(index, value);
        }

        public string LoadProperty(SqliteReader reader, int index)
        {
            return reader.ColumnText(index) ?? string.Empty;
        }
    }
}