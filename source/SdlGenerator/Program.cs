// See https://aka.ms/new-console-template for more information
using CppAst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Go(args[0]);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }

    static string FixName(string s)
    {
        if (s.StartsWith("SDL_"))
        {
            return "Sdl" + s[4..]
                .Replace("IO", "Io")
                .Replace("GPU", "Gpu")
                .Replace("GL", "Gl")
                .Replace("Glattr", "GlAttr")
                .Replace("Glcontext", "GlContext")
                .Replace("Glprofile", "GlProfile");
        }
        
        return s;
    }

    static string FixStyle(string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;
        var builder = new StringBuilder(s.Length);
        if (!char.IsAsciiLetter(s[0]))
            builder.Append('_');
        var uppercase = true;
        for (var i = 0; i < s.Length; ++i)
        {
            if (char.IsAsciiLetter(s[i]))
            {
                if (uppercase)
                {
                    builder.Append(char.ToUpperInvariant(s[i]));
                    uppercase = false;
                }
                else
                {
                    builder.Append(char.ToLowerInvariant(s[i]));
                }
            }
            else
            {
                uppercase = true;

                if (s[i] != '_')
                    builder.Append(s[i]);
            }
        }
        return builder.ToString();
    }

    static void Go(string path)
    {
        // https://stackoverflow.com/a/2224357
        // https://stackoverflow.com/a/1217952
        var source = File.ReadAllText(path);
        var baseFolder = Path.GetDirectoryName(Path.GetDirectoryName(path));
        var folders = new string[]
        {
            "/usr/lib/gcc/x86_64-linux-gnu/11/include",
            "/usr/local/include",
            "/usr/include/x86_64-linux-gnu",
            "/usr/include"
        };
        var options = new CppParserOptions();
        options.IncludeFolders.Add(baseFolder);
        options.SystemIncludeFolders.AddRange(folders);
        options.TargetSystem = "linux";
        var compilation = CppParser.Parse(source, options);

        using var writer = File.CreateText("code.cs.txt");
        writer.WriteLine("using System.Runtime.InteropServices;");
        writer.WriteLine();
        writer.WriteLine("namespace Piranha.Jawbone.Sdl3;");

        // Print diagnostic messages
        foreach (var message in compilation.Diagnostics.Messages)
            Console.WriteLine(message);

        var newNameByOldName = new Dictionary<string, string>
        {
            ["Uint8"] = "byte",
            ["Uint16"] = "ushort",
            ["Uint32"] = "uint",
            ["Uint64"] = "ulong",
            ["Sint8"] = "sbyte",
            ["Sint16"] = "short",
            ["Sint32"] = "int",
            ["Sint64"] = "long",
            ["unsigned int"] = "uint",
            ["unsigned short"] = "ushort",
            ["size_t"] = "nuint",
            ["long long"] = "long",
            ["unsigned long long"] = "ulong",
            ["unsigned long"] = "ulong" // I hate Windows
        };

        // Print All enums
        foreach (var cppEnum in compilation.Enums)
        {
            Console.WriteLine(cppEnum);

            var newName = FixName(cppEnum.Name);
            newNameByOldName.Add(cppEnum.Name, newName);
            writer.WriteLine();
            writer.Write("public enum ");
            writer.WriteLine(newName + " // " + cppEnum.Name);
            writer.WriteLine("{");

            var items = cppEnum.Items;
            var skip = 0;

            if (1 < items.Count)
            {
                while (skip < items[0].Name.Length)
                {
                    var c = items[0].Name[skip];
                    var matches = true;

                    for (int i = 1; i < items.Count; ++i)
                    {
                        var name = items[i].Name;
                        if (skip == name.Length || name[skip] != c)
                        {
                            matches = false;
                            break;
                        }
                    }

                    if (!matches)
                        break;
                    
                    ++skip;
                }
            }


            foreach (var item in cppEnum.Items)
            {
                Console.WriteLine("  - " + item);
                writer.WriteLine("    " + FixStyle(item.Name[skip..]) + " = " + item.Value + ", // " + item.Name);
            }

            writer.WriteLine("}");
        }

        // Print All typedefs
        foreach (var cppTypedef in compilation.Typedefs)
        {
            Console.WriteLine(cppTypedef);

            if ((cppTypedef.ElementType.TypeKind & CppTypeKind.Pointer) == CppTypeKind.Pointer)
            {
                newNameByOldName.Add(cppTypedef.Name, "nint");
            }
            else 
            {
                var name = cppTypedef.ElementType.GetDisplayName();
                if (newNameByOldName.TryGetValue(name, out var oldName))
                    newNameByOldName.Add(cppTypedef.Name, oldName);
                else
                    newNameByOldName.TryAdd(cppTypedef.Name, name);
            }
        }

        foreach (var cppClass in compilation.Classes)
        {
            var newName = FixName(cppClass.Name);
            newNameByOldName.Add(cppClass.Name, newName);
        }
        
        // Print All classes, structs
        foreach (var cppClass in compilation.Classes)
        {
            Console.WriteLine(cppClass);

            if (cppClass.Fields.Count == 0)
                continue;

            var newName = newNameByOldName[cppClass.Name];

            writer.WriteLine();
            writer.WriteLine("public struct " + newName + " // " + cppClass.Name);
            writer.WriteLine("{");

            foreach (var field in cppClass.Fields)
            {
                Console.WriteLine("  - " + field);

                //if (!)

                try
                {
                    var oldTypeName = field.Type.GetDisplayName();
                    var newFieldName = FixStyle(field.Name);
                    var type = (field.Type.TypeKind & CppTypeKind.Pointer) == CppTypeKind.Pointer ? "nint" : oldTypeName;
                    if (newNameByOldName.TryGetValue(type, out var alt))
                        type = alt;
                    writer.WriteLine($"    public {type} {newFieldName}; // {oldTypeName} {field.Name}");
                }
                catch
                {
                    Console.WriteLine("Uh oh. Problem with " + field.Name);
                    throw;
                }
            }
            
            writer.WriteLine("}");
        }

        writer.WriteLine();
        writer.WriteLine("public static partial class Sdl");
        writer.WriteLine("{");
            writer.WriteLine("    public const string Lib = \"SDL3\";");
        // Print All functions
        foreach (var cppFunction in compilation.Functions)
        {
            Console.WriteLine(cppFunction);
            if (!cppFunction.Name.StartsWith("SDL_"))
                continue;

            writer.WriteLine();
            writer.WriteLine($"    [LibraryImport(Lib, EntryPoint = \"{cppFunction.Name}\")]");

            var displayName = cppFunction.ReturnType.GetDisplayName();
            var returnType = (cppFunction.ReturnType.TypeKind & CppTypeKind.Pointer) == CppTypeKind.Pointer ?
                "nint" : newNameByOldName.GetValueOrDefault(displayName, displayName);
            var newName = FixStyle(cppFunction.Name[4..]);
            writer.WriteLine($"    public static partial {returnType} {newName}();");

        }

        writer.WriteLine("}");

        var divider = new string('-', 60);
        Console.WriteLine(divider);
        foreach (var pair in newNameByOldName)
            Console.WriteLine($"{pair.Key} : {pair.Value}");
        Console.WriteLine(divider);
        
        Console.WriteLine($"{compilation.Functions.Count} functions");
    }
}