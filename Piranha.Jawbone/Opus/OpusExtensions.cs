using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Opus;

public static class OpusExtensions
{
    public static IServiceCollection AddOpus(this IServiceCollection services)
    {
        return services.AddSingleton<OpusProvider>(_ => OpusProvider.Create());
    }
}