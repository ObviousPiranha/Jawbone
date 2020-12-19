using Xunit;

namespace Piranha.Jawbone.Test
{
    [CollectionDefinition(JawboneServiceCollection.Name)]
    public class JawboneServiceCollection : ICollectionFixture<ServiceFixture>
    {
        public const string Name = "Service Collection";
    }
}
