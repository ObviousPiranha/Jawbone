using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Piranha.Jawbone.SourceGenerator;

[Generator]
public sealed class LibraryImportGenerator : IIncrementalGenerator
{
    private static readonly SymbolDisplayFormat _symbolDisplayFormat = new(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var mappableClasses = context.SyntaxProvider.CreateSyntaxProvider(
            FindMappableClasses, PrepareMethods);
        
        context.RegisterSourceOutput(
            mappableClasses, GenerateSource);
    }

    private static void GenerateSource(
        SourceProductionContext context,
        LibraryClass libraryClass)
    {
        var builder = new StringBuilder();

        builder
            .AppendLine("// AUTO-GENERATED")
            .AppendLine();
        
        if (0 < libraryClass.Namespace.Length)
        {
            builder
                .Append("namespace ")
                .Append(libraryClass.Namespace)
                .AppendLine(";")
                .AppendLine();
        }

        builder
            .Append("sealed unsafe partial class ")
            .AppendLine(libraryClass.ClassName)
            .AppendLine("{");
        
        var classIndent = new IndentState(4, 1);
        var methodIndent = classIndent.Indent();

        builder
            .Indent(classIndent)
            .Append("public ")
            .Append(libraryClass.ClassName)
            .AppendLine("(")
            .Indent(methodIndent)
            .AppendLine("global::System.Func<string, nint> loader)")
            .Indent(classIndent)
            .AppendLine("{");
        
        var seenMethods = new HashSet<string>();
        foreach (var libraryMethod in libraryClass.Methods)
        {
            if (!seenMethods.Add(libraryMethod.MethodName))
                continue;
            
            builder
                .Indent(methodIndent)
                .Append(libraryMethod.FunctionPointerName)
                .Append(" = loader.Invoke(nameof(")
                .Append(libraryMethod.MethodName)
                .AppendLine(")).ToPointer();");
        }

        builder
            .Indent(classIndent)
            .AppendLine("}")
            .AppendLine();
        
        seenMethods.Clear();
        foreach (var libraryMethod in libraryClass.Methods)
        {
            if (!seenMethods.Add(libraryMethod.MethodName))
                continue;
            
            builder
                .Indent(classIndent)
                .Append("private readonly void* ")
                .Append(libraryMethod.FunctionPointerName)
                .AppendLine(";");
        }
        
        foreach (var libraryMethod in libraryClass.Methods)
        {
            var returnsVoid = libraryMethod.ReturnType == "void";
            builder
                .AppendLine()
                .Indent(classIndent)
                .Append("public partial ")
                .Append(libraryMethod.ReturnType)
                .Append(' ')
                .Append(libraryMethod.MethodName)
                .Append('(');
            
            var parameters = libraryMethod.Parameters;
            if (0 < parameters.Count)
            {
                builder
                    .AppendLine()
                    .Indent(methodIndent)
                    .Append(parameters[0].FullDefinition);
                for (int i = 1; i < parameters.Count; ++i)
                {
                    builder
                        .AppendLine(",")
                        .Indent(methodIndent)
                        .Append(parameters[i].FullDefinition);
                }
            }

            builder
                .AppendLine(")")
                .Indent(classIndent)
                .AppendLine("{");
            
            var passRefs = false;

            foreach (var parameter in parameters.Where(p => p.PassesByReference))
            {
                builder
                    .Indent(methodIndent)
                    .Append("fixed (void* ")
                    .Append(parameter.GetFinalName())
                    .Append(" = &")
                    .Append(parameter.Name)
                    .AppendLine(")");
                passRefs = true;
            }

            if (passRefs)
            {
                builder
                    .Indent(methodIndent)
                    .AppendLine("{");
            }

            var bodyIndent = passRefs ? methodIndent.Indent() : methodIndent;
            
            builder
                .Indent(bodyIndent)
                .Append("var __fp = (")
                .Append(libraryMethod.FunctionPointerType)
                .Append(")")
                .Append(libraryMethod.FunctionPointerName)
                .AppendLine(";")
                .Indent(bodyIndent)
                .Append(returnsVoid ? "" : "var __result = ")
                .Append("__fp(");
            
            if (0 < parameters.Count)
            {
                builder.Append(parameters[0].GetFinalName());
                for (int i = 1; i < parameters.Count; ++i)
                    builder.Append(", ").Append(parameters[i].GetFinalName());
            }

            builder
                .AppendLine(");");
            
            if (!returnsVoid)
            {
                builder
                    .Indent(bodyIndent)
                    .AppendLine("return __result;");
            }

            if (passRefs)
            {
                builder
                    .Indent(methodIndent)
                    .AppendLine("}");
            }

            builder
                .Indent(classIndent)
                .AppendLine("}");
        }

        builder.AppendLine("}");

        context.AddSource(
            libraryClass.ClassName + ".g.cs",
            builder.ToString());
    }

    private static bool FindMappableClasses(
        SyntaxNode syntaxNode,
        CancellationToken cancellationToken)
    {
        return
            syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
            classDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword) &&
            classDeclarationSyntax.Modifiers.Any(SyntaxKind.SealedKeyword);
    }

    private static LibraryClass PrepareMethods(
        GeneratorSyntaxContext generatorSyntaxContext,
        CancellationToken cancellationToken)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)generatorSyntaxContext.Node;
        var result = new LibraryClass
        {
            Namespace = GetNamespace(classDeclarationSyntax),
            ClassName = classDeclarationSyntax.Identifier.ValueText
        };

        var semanticModel = generatorSyntaxContext.SemanticModel;

        var symbolDisplayFormat = new SymbolDisplayFormat(
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        var methods = classDeclarationSyntax.Members
            .OfType<MethodDeclarationSyntax>()
            .Where(mds => mds.Modifiers.Any(SyntaxKind.PartialKeyword) && mds.Body is null);

        foreach (var method in methods)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var libraryMethod = new LibraryMethod
            {
                MethodName = method.Identifier.ValueText,
                ReturnType = GetTypeName(semanticModel, method.ReturnType, cancellationToken)
            };

            var functionPointerTypeBuilder = new StringBuilder(
                "delegate* unmanaged[Cdecl]<");
            
            foreach (var parameter in method.ParameterList.Parameters)
            {
                var libraryParameter = new LibraryParameter
                {
                    Name = parameter.Identifier.ValueText,
                    FullDefinition = GetDefinition(semanticModel, parameter, cancellationToken),
                    PassesByReference = IsPassByReference(parameter)
                };

                if (libraryParameter.PassesByReference)
                {
                    functionPointerTypeBuilder.Append("void*");
                }
                else
                {
                    var typeName = GetTypeName(
                        semanticModel,
                        parameter.Type,
                        cancellationToken);
                    functionPointerTypeBuilder.Append(typeName);
                }

                functionPointerTypeBuilder.Append(", ");
                
                libraryMethod.Parameters.Add(libraryParameter);
            }

            functionPointerTypeBuilder
                .Append(libraryMethod.ReturnType)
                .Append('>');
            
            libraryMethod.FunctionPointerType = functionPointerTypeBuilder.ToString();

            result.Methods.Add(libraryMethod);
        }

        return result;
    }

    private static string GetDefinition(
        SemanticModel semanticModel,
        ParameterSyntax parameterSyntax,
        CancellationToken cancellationToken)
    {
        var type = semanticModel.GetTypeInfo(parameterSyntax.Type, cancellationToken).Type;
        var typeString = type.ToDisplayString(_symbolDisplayFormat);

        if (typeString.Contains('.'))
            typeString = "global::" + typeString;
        
        var result = typeString + " " + parameterSyntax.Identifier.ValueText;
        var modifiers = parameterSyntax.Modifiers;
        if (modifiers.Any(SyntaxKind.ReadOnlyKeyword))
            result = "readonly " + result;
        if (modifiers.Any(SyntaxKind.RefKeyword))
            result = "ref " + result;
        if (modifiers.Any(SyntaxKind.InKeyword))
            result = "in " + result;
        if (modifiers.Any(SyntaxKind.OutKeyword))
            result = "out " + result;
        return result;
    }

    private static bool IsPassByReference(ParameterSyntax parameterSyntax)
    {
        var m = parameterSyntax.Modifiers;
        return
            m.Any(SyntaxKind.InKeyword) ||
            m.Any(SyntaxKind.OutKeyword) ||
            m.Any(SyntaxKind.RefKeyword);
    }

    private static string GetTypeName(
        SemanticModel semanticModel,
        ExpressionSyntax expressionSyntax,
        CancellationToken cancellationToken)
    {
        var type = semanticModel.GetTypeInfo(expressionSyntax, cancellationToken).Type;
        return type.ToDisplayString(_symbolDisplayFormat);
    }

    private static string GetNamespace(BaseTypeDeclarationSyntax syntax)
    {
        // https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/
        var result = "";

        var candidate = syntax.Parent;
        while (
            candidate is not null &&
            candidate is not NamespaceDeclarationSyntax &&
            candidate is not FileScopedNamespaceDeclarationSyntax)
        {
            candidate = candidate.Parent;
        }

        if (candidate is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            result = namespaceParent.Name.ToString();

            while (namespaceParent.Parent is NamespaceDeclarationSyntax parent)
            {
                result = $"{namespaceParent.Name}.{result}";
                namespaceParent = parent;
            }
        }

        return result;
    }
}