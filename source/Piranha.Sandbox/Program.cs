using Jawbone;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Piranha.Sandbox;

class Program
{
    static void Dump(CsvReader reader)
    {
        Console.WriteLine(string.Join(", ", reader.GetColumnNames()));
        var row = 0;
        while (reader.TryReadRow())
        {
            Console.WriteLine("Row " + ++row);
            for (int i = 0; i < reader.FieldCount; ++i)
            {
                Console.WriteLine("  - " + reader.GetFieldUtf16(i));
            }
        }
    }
    static void Main(string[] args)
    {
        try
        {
            // using var stream = File.OpenRead("sample.csv");
            var random = Random.Shared;
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write("Id,Name,Value\n");
            var id = 0;
            for (int i = 0; i < 4000; ++i)
            {
                writer.Write((++id).ToString());
                writer.Write(',');

                for (int j = 0; j < 8; ++j)
                {
                    var offset = random.Next(26);
                    var c = (char)('a' + offset);
                    writer.Write(c);
                }

                writer.Write(',');
                var v = random.Next() - random.Next();
                writer.Write(v.ToString());
                writer.Write('\n');
            }

            writer.Flush();
            stream.Position = 0;
            {
                using var fileStream = File.Create("raw.csv");
                stream.CopyTo(fileStream);
            }
            stream.Position = 0;
            var start = Stopwatch.GetTimestamp();
            var reader = new CsvReader(stream);
            Dump(reader);
            Console.WriteLine("Completed in " + Stopwatch.GetElapsedTime(start));
            Console.WriteLine("Done");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }
}
