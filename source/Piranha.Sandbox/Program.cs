using Jawbone;
using System;
using System.IO;

namespace Piranha.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            using var stream = File.OpenRead("sample.csv");
            var reader = new CsvReader(stream);
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
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }
}
