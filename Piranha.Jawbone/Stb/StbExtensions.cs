using Microsoft.Extensions.DependencyInjection;
using Piranha.Tools;

namespace Piranha.Stb
{
    public static class StbExtensions
    {
        public static IServiceCollection AddStb(
            this IServiceCollection services,
            string dll)
        {
            return services.AddNativeLibrary<IStb>(
                _ => NativeLibraryInterface.Create<IStb>(dll, ResolveName));
        }

        public static string ResolveName(string methodName)
        {
            const string TtPrefix = "Stbtt";
            if (methodName.StartsWith(TtPrefix))
            {
                return "stbtt_" + methodName.Substring(TtPrefix.Length);
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