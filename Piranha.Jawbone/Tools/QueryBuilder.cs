using System;
using System.Collections.Generic;
using System.Text;

namespace Piranha.Jawbone.Tools;

public class QueryBuilder
{
    private readonly string _from;
    private readonly List<string> _select = new List<string>();
    private readonly List<string> _specify = new List<string>();
    private readonly List<string> _where = new List<string>();

    public QueryBuilder(string from) => _from = from;

    public QueryBuilder Select<T>(Func<T, string> selector, params T[] items)
    {
        foreach (var item in items)
        {
            var selected = selector(item);
            _select.Add(selected);
        }

        return this;
    }

    public QueryBuilder Select<T>(Func<T, string> selector, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            var selected = selector(item);
            _select.Add(selected);
        }

        return this;
    }

    public QueryBuilder Select(params string[] items)
    {
        foreach (var item in items)
            _select.Add(item);

        return this;
    }

    public QueryBuilder Select(IEnumerable<string> items)
    {
        _select.AddRange(items);
        return this;
    }

    public QueryBuilder Where(params string[] items)
    {
        foreach (var item in items)
            _where.Add(item);

        return this;
    }

    public override string ToString()
    {
        var builder = new StringBuilder("SELECT ");

        foreach (var item in _specify)
            builder.Append(item).Append(' ');

        if (_select.Count > 0)
        {
            builder.Append(_select[0]);

            for (int i = 1; i < _select.Count; ++i)
                builder.Append(", ").Append(_select[i]);
        }
        else
        {
            builder.Append('*');
        }

        builder.Append(" FROM ").Append(_from);

        if (_where.Count > 0)
        {
            builder.Append(" WHERE (").Append(_where[0]);

            for (int i = 1; i < _where.Count; ++i)
                builder.Append(") AND (").Append(_where[i]);

            builder.Append(')');
        }

        return builder.ToString();
    }
}
