using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools;
using System;

namespace Piranha.Jawbone.Stb
{
    public static class StbExtensions
    {
        public static IServiceCollection AddStb(this IServiceCollection services)
        {
            return services.AddNativeLibrary(
                _ => NativeLibraryInterface.FromFile<IStb>("PiranhaNative.dll", ResolveName));
        }

        public static string ResolveName(string methodName)
        {
            const string TtPrefix = "Stbtt";
            if (methodName.StartsWith(TtPrefix))
            {
                return string.Concat("stbtt_", methodName.AsSpan(TtPrefix.Length));
            }
            else
            {
                var chars = new char[methodName.Length * 2];
                chars[0] = char.ToLowerInvariant(methodName[0]);
                int n = 1;

                for (int i = 1; i < methodName.Length; ++i)
                {
                    char c = methodName[i];

                    if (char.IsUpper(c))
                    {
                        chars[n++] = '_';
                        chars[n++] = char.ToLowerInvariant(c);
                    }
                    else
                    {
                        chars[n++] = c;
                    }
                }

                return new string(chars, 0, n);
            }
        }
    }
}
