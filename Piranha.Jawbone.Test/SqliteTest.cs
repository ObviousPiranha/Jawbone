using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Sqlite;
using Xunit;

namespace Piranha.Jawbone.Test
{
    [Collection(JawboneServiceCollection.Name)]
    public class SqliteTest
    {
        private readonly ISqlite3 _sqlite3;

        public SqliteTest(ServiceFixture fixture)
        {
            _sqlite3 = fixture.ServiceProvider.GetRequiredService<ISqlite3>();
        }

        [Fact]
        public void HasVersion()
        {
            var version = _sqlite3.Libversion();
            Assert.NotNull(version);
        }
    }
}
