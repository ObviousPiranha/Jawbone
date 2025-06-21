using System;
using System.IO;
using System.Text.Json;

namespace Jawbone.NativeSourceGenerator;

class Program
{
    static void Main(string[] args)
    {
        string[] inputs = [
            "/home/kelly/Code/ObviousPiranha/Jawbone/source/Jawbone/Stb/StbVorbisLibrary.cs"
        ];

        foreach (var file in inputs)
        {
            var folder = Path.GetDirectoryName(file) ?? "";
            var filename = Path.GetFileNameWithoutExtension(file);
            if (file.EndsWith(".cs"))
            {

                var source = File.ReadAllText(file);
                var classSpec = ClassSpec.FromOldSource(source);

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(classSpec, options);

                var jsonFile = Path.Combine(folder, filename + ".json");
                File.WriteAllText(jsonFile, json);
                // Console.WriteLine(json);
                // Console.WriteLine();

                var newSource = classSpec.ToSource();
                File.WriteAllText(file, newSource);
                // Console.WriteLine(newSource);
            }
            else
            {
                var json = File.ReadAllText(file);
                var classSpec = JsonSerializer.Deserialize<ClassSpec>(json);
                if (classSpec is null)
                    continue;
                var sourceFile = Path.Combine(folder, filename + ".cs");
                var source = classSpec.ToSource();
                File.WriteAllText(sourceFile, source);
            }
        }
    }
}