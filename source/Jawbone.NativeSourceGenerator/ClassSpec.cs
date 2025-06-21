using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Jawbone.NativeSourceGenerator;

class ClassSpec
{
    private const string Space = " \n\r\t";

    public string? Namespace { get; set; }
    public required string ClassName { get; set; }
    public required List<List<string>> Methods { get; set; }

    public string ToSource()
    {
        var builder = new StringBuilder();

        builder
            .Append("// Code generated at ")
            .Append(DateTimeOffset.UtcNow.ToString("s"))
            .AppendLine(".")
            .AppendLine();

        if (Namespace is not null)
        {
            builder
                .Append("namespace ")
                .Append(Namespace)
                .AppendLine(";")
                .AppendLine();
        }

        builder
            .Append("public sealed unsafe class ")
            .AppendLine(ClassName)
            .AppendLine("{");


        var methodNames = new SortedSet<string>(
            Methods.Select(m => SpanReader.ReadLastWord(m[0]).ToString()));
        var classIndent = new Indent(4, 1);
        foreach (var methodName in methodNames)
        {
            builder
                .AppendIndent(classIndent)
                .Append("private readonly nint _fp_")
                .Append(methodName)
                .AppendLine(";");
        }

        builder
            .AppendLine()
            .AppendIndent(classIndent)
            .Append("public ") // start constructor
            .Append(ClassName)
            .AppendLine("(")
            .AppendIndent(classIndent.Increment())
            .AppendLine("System.Func<string, nint> loader)")
            .AppendIndent(classIndent)
            .AppendLine("{");

        var methodIndent = classIndent.Increment();
        foreach (var methodName in methodNames)
        {
            builder
                .AppendIndent(methodIndent)
                .Append("_fp_")
                .Append(methodName)
                .Append(" = loader.Invoke(nameof(")
                .Append(methodName)
                .AppendLine("));");
        }

        builder
            .AppendIndent(classIndent)
            .AppendLine("}"); // finish constructor

        foreach (var method in Methods)
        {
            builder
                .AppendLine()
                .AppendIndent(classIndent)
                .Append("public ")
                .Append(method[0])
                .Append('(');

            if (1 < method.Count)
            {
                var indent = classIndent.Increment();
                builder
                    .AppendLine()
                    .AppendIndent(indent)
                    .Append(method[1]);

                for (int i = 2; i < method.Count; ++i)
                {
                    builder
                        .AppendLine(",")
                        .AppendIndent(indent)
                        .Append(method[i]);
                }
            }

            builder
                .AppendLine(")")
                .AppendIndent(classIndent)
                .AppendLine("{");

            // builder.AppendIndent(methodIndent).AppendLine("// CODE HERE");
            var names = new List<string>();
            var types = new List<string>();
            var fixes = new List<string>();

            for (int i = 0; i < method.Count; ++i)
            {
                var component = method[i];
                var index = component.LastIndexOf(' ');
                var type = component.AsSpan(..index);
                var name = component.AsSpan(index + 1);
                if (0 < i && type.Contains(' '))
                {
                    var pointerName = string.Concat("__p_", name);
                    types.Add("void*");
                    names.Add(pointerName);
                    fixes.Add(string.Concat(pointerName, " = &", name));
                }
                else
                {
                    types.Add(type.ToString());
                    names.Add(name.ToString());
                }
            }

            builder
                .AppendIndent(methodIndent)
                .AppendLine("var __fp = (delegate* unmanaged[Cdecl]<")
                .AppendIndent(methodIndent.Increment());
            for (int i = 1; i < types.Count; ++i)
            {
                builder
                    .Append(types[i])
                    .Append(", ");
            }

            builder
                .AppendLine(types[0])
                .AppendIndent(methodIndent.Increment())
                .Append(">)_fp_")
                .Append(names[0])
                .AppendLine(";");

            var fixedIndent = methodIndent;
            if (0 < fixes.Count)
            {
                foreach (var fix in fixes)
                {
                    builder
                        .AppendIndent(methodIndent)
                        .Append("fixed (void* ")
                        .Append(fix)
                        .AppendLine(")");
                }
                builder
                    .AppendIndent(methodIndent)
                    .AppendLine("{");
                fixedIndent = methodIndent.Increment();
            }

            builder.AppendIndent(fixedIndent);
            var returnsValue = types[0] != "void";
            if (returnsValue)
                builder.Append("var __result = ");
            builder.Append("__fp(");
            if (1 < names.Count)
            {
                builder.Append(names[1]);

                for (int i = 2; i < names.Count; ++i)
                {
                    builder
                        .Append(", ")
                        .Append(names[i]);
                }
            }

            builder.AppendLine(");");

            if (returnsValue)
            {
                builder
                    .AppendIndent(fixedIndent)
                    .AppendLine("return __result;");
            }

            if (0 < fixes.Count)
            {
                builder
                    .AppendIndent(methodIndent)
                    .AppendLine("}");
            }

            builder
                .AppendIndent(classIndent)
                .AppendLine("}");
        }

        builder.AppendLine("}");
        return builder.ToString();
    }

    public static ClassSpec FromOldSource(ReadOnlySpan<char> source)
    {
        var classNamespace = default(string);
        var methodDefinitions = new List<List<string>>();
        var reader = SpanReader.Create(source);
        if (reader.TryReadSandwich("namespace ", ";", out var classNamespaceSpan))
            classNamespace = classNamespaceSpan.Trim(Space).ToString();
        if (!reader.TryReadSandwich("partial class ", "{", out var classNameSpan))
            throw new InvalidOperationException("Missing class name.");
        var className = classNameSpan.Trim(Space).ToString();
        while (reader.TryReadSandwich("public partial ", ";", out var method))
        {
            if (!SpanReader.TryReadSandwich(method, '(', ')', out var parameters))
                throw new InvalidOperationException("Missing parameter list.");

            var methodDefinition = new List<string>
            {
                SpanReader.ReadUntil(method, '(').Trim(Space).ToString()
            };

            foreach (var range in parameters.Split(','))
            {
                var parameter = parameters[range].Trim(Space);
                if (parameter.IsEmpty)
                    continue;
                methodDefinition.Add(parameter.ToString());
            }

            methodDefinitions.Add(methodDefinition);
        }

        methodDefinitions.Sort(CompareLastWord);

        var result = new ClassSpec
        {
            Namespace = classNamespace,
            ClassName = className,
            Methods = methodDefinitions
        };

        return result;
    }

    private static int CompareLastWord(List<string> a, List<string> b)
    {
        var word0 = SpanReader.ReadLastWord(a[0]);
        var word1 = SpanReader.ReadLastWord(b[0]);
        var result = word0.SequenceCompareTo(word1);
        if (result == 0)
            result = a.Count.CompareTo(b.Count);
        return result;
    }
}