using System.Collections.Immutable;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sqlite
{
    class ImmutableByteArrayHandler : ITypeHandler<ImmutableArray<byte>>
    {
        public string DataType => TypeHandler.Blob;

        public void BindProperty(SqliteStatement statement, int index, ImmutableArray<byte> value)
        {
            statement.BindBlob(index, value.AsSpan());
        }

        public ImmutableArray<byte> LoadProperty(SqliteReader reader, int index)
        {
            var blob = reader.ColumnBlob(index);
            return ImmutableArrayFactory.Create(blob);
        }
    }
}
