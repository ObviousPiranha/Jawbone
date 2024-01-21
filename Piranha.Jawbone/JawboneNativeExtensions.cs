using Microsoft.Extensions.DependencyInjection;

namespace Piranha.Jawbone;

public static class JawboneNativeExtensions
{
    public static IServiceCollection AddJawboneNativeLibraries(
        this IServiceCollection services)
    {
        return services
            .AddSingleton(_ => new JawboneNative("./PiranhaNative.dll"))
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().Sqlite3)
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().StbImage)
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().StbImageWrite)
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().StbTrueType)
            .AddSingleton(
                serviceProvider => serviceProvider.GetRequiredService<JawboneNative>().StbVorbis);
    }
}