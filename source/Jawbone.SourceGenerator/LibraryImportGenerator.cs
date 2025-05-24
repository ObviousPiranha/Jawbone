using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jawbone.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class LibraryImportGenerator : IIncrementalGenerator
{
    private const int IndentSize = 4;
    private const string AttributeName = "MapNativeFunctionsAttribute";
    private const string AttributeNamespace = "Jawbone.Generation";
    private const string AttributeFullName = AttributeNamespace + "." + AttributeName;

    private static readonly SymbolDisplayFormat _symbolDisplayFormat = new(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var mappableClasses = context.SyntaxProvider.CreateSyntaxProvider(
            FindPartialSealedClasses, PrepareMethods);

        var mappedClasses = mappableClasses
            .Where(libraryClass => libraryClass is not null)
            .Select((libraryClass, _) => libraryClass!);

        context.RegisterSourceOutput(
            mappedClasses, GenerateSource);
        context.RegisterPostInitializationOutput(
            GenerateAttributeSource);
    }

    private static void GenerateAttributeSource(
        IncrementalGeneratorPostInitializationContext context)
    {
        var classIndent = new IndentState(IndentSize, 1);
        var methodIndent = classIndent.Indent();
        var builder = new StringBuilder();

        builder
            .AppendLine("// AUTO-GENERATED")
            .AppendLine()
            .AppendLine("using System;")
            .AppendLine()
            .Append("namespace ")
            .Append(AttributeNamespace)
            .AppendLine(";")
            .AppendLine()
            .AppendLine("[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]")
            .Append("sealed class ")
            .Append(AttributeName)
            .AppendLine(" : Attribute")
            .AppendLine("{")
            .AppendLine("}");
        context.AddSource(
            AttributeName + ".cs",
            builder.ToString());
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

        var classIndent = new IndentState(IndentSize, 1);
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
                .AppendLine("));");
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
                .Append("private readonly nint ")
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
                    .Append(parameters[0].MethodDefinition);
                for (int i = 1; i < parameters.Count; ++i)
                {
                    builder
                        .AppendLine(",")
                        .Indent(methodIndent)
                        .Append(parameters[i].MethodDefinition);
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
            var typeIndent = bodyIndent.Indent();

            builder
                .Indent(bodyIndent)
                .AppendLine("var __fp = (delegate* unmanaged[Cdecl]<");

            foreach (var libraryParameter in libraryMethod.Parameters)
            {
                builder
                    .Indent(typeIndent)
                    .Append(libraryParameter.FunctionPointerType)
                    .AppendLine(",");
            }
            builder
                .Indent(typeIndent)
                .AppendLine(libraryMethod.ReturnType)
                .Indent(typeIndent)
                .Append(">)")
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

    private static bool FindPartialSealedClasses(
        SyntaxNode syntaxNode,
        CancellationToken cancellationToken)
    {
        return
            syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
            classDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword) &&
            classDeclarationSyntax.Modifiers.Any(SyntaxKind.SealedKeyword);
    }

    private static LibraryClass? PrepareMethods(
        GeneratorSyntaxContext generatorSyntaxContext,
        CancellationToken cancellationToken)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)generatorSyntaxContext.Node;
        var semanticModel = generatorSyntaxContext.SemanticModel;

        if (!classDeclarationSyntax.AttributeLists.Any(
            als => als.Attributes.Any(
                att => semanticModel.GetTypeInfo(att, cancellationToken).Type?.ToDisplayString(_symbolDisplayFormat) == AttributeFullName)))
        {
            return null;
        }

        var result = new LibraryClass
        {
            Namespace = GetNamespace(classDeclarationSyntax),
            ClassName = classDeclarationSyntax.Identifier.ValueText
        };

        var symbolDisplayFormat = new SymbolDisplayFormat(
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        var methods = classDeclarationSyntax.Members
            .OfType<MethodDeclarationSyntax>()
            .Where(mds => mds.Modifiers.Any(SyntaxKind.PartialKeyword) && mds.Body is null);

        foreach (var method in methods)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var returnType = GetTypeName(semanticModel, method.ReturnType, cancellationToken);
            if (returnType.Contains('.'))
                returnType = "global::" + returnType;

            var libraryMethod = new LibraryMethod
            {
                MethodName = method.Identifier.ValueText,
                ReturnType = returnType
            };

            foreach (var parameter in method.ParameterList.Parameters)
            {
                var libraryParameter = new LibraryParameter
                {
                    Name = parameter.Identifier.ValueText,
                    MethodDefinition = GetDefinition(semanticModel, parameter, cancellationToken),
                    PassesByReference = IsPassByReference(parameter)
                };

                if (libraryParameter.PassesByReference)
                {
                    libraryParameter.FunctionPointerType = "void*";
                }
                else
                {
                    var typeName = GetTypeName(
                        semanticModel,
                        parameter.Type.CannotBeNull(),
                        cancellationToken);

                    if (typeName.Contains('.'))
                        typeName = "global::" + typeName;

                    libraryParameter.FunctionPointerType = typeName;
                }

                libraryMethod.Parameters.Add(libraryParameter);
            }

            result.Methods.Add(libraryMethod);
        }

        return result;
    }

    private static string GetDefinition(
        SemanticModel semanticModel,
        ParameterSyntax parameterSyntax,
        CancellationToken cancellationToken)
    {
        var type = semanticModel.GetTypeInfo(
            parameterSyntax.Type.CannotBeNull(),
            cancellationToken).Type.CannotBeNull();
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
        var type = semanticModel.GetTypeInfo(expressionSyntax, cancellationToken).Type.CannotBeNull();
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
