using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Sqlite;
using System;

namespace Piranha.Jawbone.Test.Native;

// https://xunit.net/docs/shared-context

public sealed class ServiceFixture : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public IServiceProvider ServiceProvider => _serviceProvider;

    public ServiceFixture()
    {
        var services = new ServiceCollection();
        services.AddSqlite3();

        var options = new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        };

        _serviceProvider = services.BuildServiceProvider(options);
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }
}
