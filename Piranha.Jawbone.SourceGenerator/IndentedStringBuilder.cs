using System;
using System.Collections.Generic;
using System.Text;

namespace Piranha.Jawbone.SourceGenerator;

class IndentedStringBuilder
{
    public const int DefaultSpacesPerIndent = 4;

    private readonly StringBuilder _builder;
    private readonly int _indentLevel;
    private readonly int _spacesPerLevel;

    private IndentedStringBuilder(
        StringBuilder builder,
        int indentLevel,
        int spacesPerLevel)
    {
        _builder = builder;
        _indentLevel = indentLevel;
        _spacesPerLevel = spacesPerLevel;
    }

    private StringBuilder AppendIndent() => _builder.Append(' ', _indentLevel * _spacesPerLevel);

    public IndentedStringBuilder AppendLine(Action<StringBuilder>? action)
    {
        AppendIndent();
        action?.Invoke(_builder);
        _builder.AppendLine();
        return this;
    }

    public IndentedStringBuilder AppendLine(string? content = null)
    {
        if (content is null)
            _builder.AppendLine();
        else
            AppendIndent().AppendLine(content);
        return this;
    }

    public IndentedStringBuilder AppendLines(IEnumerable<string?>? items)
    {
        if (items is not null)
        {
            foreach (var item in items)
                AppendLine(item);
        }

        return this;
    }

    public IndentedStringBuilder AppendBlock(
        string? before,
        string? after,
        Action<IndentedStringBuilder>? action)
    {
        if (before is not null)
            AppendLine(before);
        
        AppendBlock(action);
        
        if (after is not null)
            AppendLine(after);
        
        return this;
    }

    public IndentedStringBuilder AppendBlock(Action<IndentedStringBuilder>? action)
    {
        if (action is not null)
        {
            var blockBuilder = new IndentedStringBuilder(_builder, _indentLevel + 1, _spacesPerLevel);
            action.Invoke(blockBuilder);
        }

        return this;
    }

    public override string ToString() => _builder.ToString();

    public static IndentedStringBuilder Create(int spacesPerLevel = DefaultSpacesPerIndent) => new(new(), 0, spacesPerLevel);
}