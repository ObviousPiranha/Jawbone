
namespace Jawbone.Test.Native;

[CollectionDefinition(JawboneServiceCollection.Name)]
public class JawboneServiceCollection : ICollectionFixture<ServiceFixture>
{
    public const string Name = "Service Collection";
}
