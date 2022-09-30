using System;

namespace Piranha.Jawbone.Sqlite
{
    class DateTimeHandler : ITypeHandler<DateTime>
    {
        public string DataType => TypeHandler.Text;

        public void BindProperty(SqliteStatement statement, int index, DateTime value)
        {
            statement.BindText(index, value.ToString("s"));
        }

        public DateTime LoadProperty(SqliteReader reader, int index)
        {
            var text = reader.ColumnUtf16(index);
            DateTime.TryParse(text, out var result);
            return result;
        }
    }
}
