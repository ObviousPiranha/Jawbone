using System;

namespace Piranha.Sqlite
{
    class GuidHandler : ITypeHandler<Guid>
    {
        public string DataType => TypeHandler.Text;

        public void BindProperty(SqliteStatement statement, int index, Guid value)
        {
            statement.BindText(index, value.ToString());
        }

        public Guid LoadProperty(SqliteReader reader, int index)
        {
            var text = reader.ColumnText(index);
            Guid.TryParse(text, out var result);
            return result;
        }
    }
}