using System;
using System.Buffers;
using System.IO;

internal class Program
{
    static readonly SearchValues<byte> Gap = SearchValues.Create(" \t\r\n"u8);

    private static int Main(string[] args)
    {
        try
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Missing command.");
                return 0;
            }

            switch (args[0])
            {
                case "fix-namespace":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Missing target file.");
                    }
                    else
                    {
                        FixNamespace(args[1]);
                    }
                    break;
                case "fix-namespace-folder":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Missing target folder.");
                    }
                    else
                    {
                        FixNamespaceFolder(args[1]);
                    }
                    break;
                default:
                    Console.WriteLine("Unrecognized command: " + args[0]);
                    break;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
            return 1;
        }
    }

    static void FixNamespaceFolder(string folder)
    {
        foreach (var file in Directory.EnumerateFiles(folder, "*.cs"))
            FixNamespace(file);

        foreach (var innerFolder in Directory.EnumerateDirectories(folder))
        {
            var folderName = Path.GetFileName(innerFolder.AsSpan());
            if (folderName.SequenceEqual("bin") || folderName.SequenceEqual("obj"))
                continue;
        }
    }

    static void FixNamespace(string file)
    {
        Console.WriteLine(file);
        var bytes = File.ReadAllBytes(file);
        var span = bytes.AsSpan();
        var keyword = "namespace"u8;
        var nsIndex = span.IndexOf(keyword);
        if (nsIndex == -1)
            return;
        var afterNsIndex = nsIndex + keyword.Length;
        var afterNs = span[afterNsIndex..];
        var firstBrace = afterNs.IndexOf((byte)'{');
        if (firstBrace == -1)
            return;
        var firstSemicolon = afterNs.IndexOf((byte)';');
        if (0 <= firstSemicolon && firstSemicolon < firstBrace)
            return;
        var lastBrace = span.LastIndexOf((byte)'}');
        if (lastBrace == -1)
            return;
        var lineEnding = span.Contains((byte)'\r') ? "\r\n"u8 : "\n"u8;
        var headEnd = afterNsIndex + firstBrace;
        while (Gap.Contains(span[headEnd - 1]))
            --headEnd;
        var bodySpan = span[(afterNsIndex + firstBrace + 1)..lastBrace];
        using var stream = File.Create(file);
        stream.Write(span[..headEnd]);
        stream.Write(";"u8);
        stream.Write(lineEnding);
        //stream.Write(bodySpan);
        var remaining = bodySpan;
        while (!remaining.IsEmpty)
        {
            var lei = remaining.IndexOf((byte)'\n');
            if (lei == -1)
            {
                stream.Write(remaining);
                stream.Write(lineEnding);
                break;
            }
            else
            {
                var line = remaining.Slice(0, lei + 1);
                if (line.StartsWith("    "u8))
                    line = line[4..];
                stream.Write(line);
                remaining = remaining.Slice(lei + 1);
            }
        }
    }
}
