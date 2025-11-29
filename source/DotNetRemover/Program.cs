namespace DotNetRemover;

class Program
{
    private const string DotNetRoot = "DOTNET_ROOT";

    private static void Main(string[] args)
    {
        try
        {
            var origin = "";
            var dotNetFolder = default(string);
            if (0 < args.Length && !string.IsNullOrWhiteSpace(args[0]))
            {
                var folder = args[0].Trim();
                if (Directory.Exists(folder))
                {
                    dotNetFolder = folder;
                    origin = "command-line argument";
                }
                else
                {
                    Console.WriteLine($"Command-line argument folder [{folder}] does not exist.");
                }
            }

            if (dotNetFolder is null)
            {
                var folder = Environment.GetEnvironmentVariable(DotNetRoot)?.Trim();

                if (folder is not null && 0 < folder.Length)
                {
                    if (Directory.Exists(folder))
                    {
                        dotNetFolder = folder;
                        origin = "environment variable " + DotNetRoot;
                    }
                    else
                    {
                        Console.WriteLine($"Environment variable {DotNetRoot} folder [{folder}] does not exist.");
                    }
                }
            }

            if (dotNetFolder is null)
            {
                var homeFolder = Environment.GetEnvironmentVariable("HOME");
                if (homeFolder is not null)
                {
                    var folder = Path.Combine(homeFolder, ".dotnet");
                    if (Directory.Exists(folder))
                    {
                        dotNetFolder = folder;
                        origin = "default location";
                    }
                    else
                    {
                        Console.WriteLine($"Default folder [{folder}] does not exist.");
                    }
                }
            }

            if (dotNetFolder is null)
                return;

            Console.WriteLine($"Targeting folder [{dotNetFolder}] provided by {origin}.");

            var sdkFolder = Path.Combine(dotNetFolder, "sdk");

            if (!Directory.Exists(sdkFolder))
            {
                Console.WriteLine($"SDK folder [{sdkFolder}] not found.");
                return;
            }

            var toDelete = new List<string>();

            var sdkVersions = Directory.GetDirectories(sdkFolder);
            Array.Sort(sdkVersions, OrderVersionFoldersDescending);
            Console.WriteLine("---- SDK versions ----");
            foreach (var sdkVersion in sdkVersions)
            {
                var version = Path.GetFileName(sdkVersion.AsSpan());
                Console.WriteLine(string.Concat("  - ", version));
            }

            var sdkMajorVersions = Array.ConvertAll(sdkVersions, GetMajorVersionFromFolder);
            for (int i = 1; i < sdkMajorVersions.Length; ++i)
            {
                if (sdkMajorVersions[i] == sdkMajorVersions[i - 1])
                    toDelete.Add(sdkVersions[i]);
            }

            var sharedFolder = Path.Combine(dotNetFolder, "shared");
            if (Directory.Exists(sharedFolder))
            {
                var folders = Directory.GetDirectories(sharedFolder);

                foreach (var folder in folders)
                {
                    var runtime = Path.GetFileName(folder);
                    Console.WriteLine($"---- .NET runtime {folder} ----");
                    var versionFolders = Directory.GetDirectories(folder);
                    Array.Sort(versionFolders, OrderVersionFoldersDescending);
                    foreach (var versionFolder in versionFolders)
                    {
                        var version = Path.GetFileName(versionFolder.AsSpan());
                        Console.WriteLine(string.Concat("  - ", version));
                    }

                    var majorVersionFolders = Array.ConvertAll(versionFolders, GetMajorVersionFromFolder);
                    for (int i = 1; i < majorVersionFolders.Length; ++i)
                    {
                        if (majorVersionFolders[i] == majorVersionFolders[i - 1])
                            toDelete.Add(versionFolders[i]);
                    }
                }
            }

            Console.WriteLine("---- Suggested Deletion ----");
            foreach (var item in toDelete)
            {
                Console.WriteLine("  - " + item);
            }

            Console.WriteLine("Delete these folders?");
            var input = Console.ReadLine() ?? "";

            if (0 < input.Length && char.ToLowerInvariant(input[0]) == 'y')
            {
                foreach (var item in toDelete)
                {
                    Console.WriteLine("Deleting " + item);
                    Directory.Delete(item, true);
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }

    private static string GetMajorVersionFromFolder(string folder)
    {
        var version = Path.GetFileName(folder.AsSpan());
        var result = SpanReader.TrimAt(version, '.');
        return result.ToString();
    }

    private static int OrderVersionFoldersDescending(string? a, string? b) => OrderVersionFoldersAscending(b, a);

    private static int OrderVersionFoldersAscending(string? a, string? b)
    {
        var versionA = Path.GetFileName(a.AsSpan());
        var versionB = Path.GetFileName(b.AsSpan());
        var result = CompareVersions(versionA, versionB);
        return result;
    }

    private static int CompareVersions(string? a, string? b) => CompareVersions(a.AsSpan(), b.AsSpan());

    private static int CompareVersions(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        var readerA = SpanReader.Create(a);
        var readerB = SpanReader.Create(b);

        while (!readerA.Completed || !readerB.Completed)
        {
            var blockA = readerA.ReadUntilValueOrEnd('.');
            var blockB = readerB.ReadUntilValueOrEnd('.');

            _ = int.TryParse(blockA, out var versionA);
            _ = int.TryParse(blockB, out var versionB);

            var comparison = versionA.CompareTo(versionB);
            if (comparison != 0)
                return comparison;
        }

        return readerA.Completed ? -1 : readerB.Completed ? 1 : 0;
    }
}