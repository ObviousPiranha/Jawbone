using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Piranha.Jawbone.SourceGenerator;

[Generator]
public class NativeInteropSourceGenerator : ISourceGenerator
{
    // https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview
    public void Execute(GeneratorExecutionContext context)
    {
        // https://stackoverflow.com/a/65126680
        var classNames = new List<string>();
        foreach (var syntaxTree in context.Compilation.SyntaxTrees)
        {
            var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
            var classes = syntaxTree
                .GetRoot()
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(classDeclarationSyntax => classDeclarationSyntax
                    .AttributeLists.Any());
                // .Where(structDeclarationSyntax => structDeclarationSyntax
                //     .DescendantNodes()
                //     .OfType<AttributeSyntax>()
                //     .Any(a => a.DescendantTokens().Any(
                //         dt => dt.IsKind(SyntaxKind.IdentifierToken) &&
                //         dt.Parent is not null &&
                //         semanticModel.GetTypeInfo(dt.Parent).Type?.Name == "LibraryInterface")));
                
                // https://stackoverflow.com/a/33095466
                // classNames.AddRange(classes.Select(c => semanticModel.GetDeclaredSymbol(c)?.ToDisplayString()).WhereNotNull());
                foreach (var c in classes)
                {
                    var displayName = semanticModel.GetDeclaredSymbol(c)?.ToDisplayString();

                    if (displayName is null)
                        continue;
                    
                    var attributes = c.AttributeLists
                        .SelectMany(attributeList => attributeList.Attributes.Select(a => semanticModel.GetTypeInfo(a).Type?.ToDisplayString()))
                        .WhereNotNull();
                    
                    classNames.Add(displayName + " - " + string.Join(", ", attributes));
                }
        }

        var builder = IndentedStringBuilder.Create();
        builder
            .AppendLine(sb => sb.Append("// Generated by Jawbone at ").Append(DateTime.Now.ToString("s")))
            .AppendLine()
            .AppendBlock("/**", "*/", block => block.AppendLines(classNames));
        context.AddSource("NativeLibraries.g.cs", builder.ToString());
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}