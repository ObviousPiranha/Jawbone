using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Jawbone;

public sealed class OneToMany<TOne, TMany>
    where TOne : notnull
    where TMany : notnull
{
    private readonly IEqualityComparer<TOne> _oneEquality;
    private readonly IEqualityComparer<TMany> _manyEquality;
    private readonly Dictionary<TOne, ImmutableArray<TMany>> _oneToMany;
    private readonly Dictionary<TMany, TOne> _manyToOne;

    public int Count => _oneToMany.Count;

    public TOne this[TMany many]
    {
        get => _manyToOne[many];
        set
        {
            if (_manyToOne.TryGetValue(many, out var one))
            {
                if (_oneEquality.Equals(one, value))
                    return;

                var manyValues = _oneToMany[one];
                var removedMany = manyValues.Remove(many, _manyEquality);
                Debug.Assert(removedMany.Length < manyValues.Length);
                Many.SetOrRemove(_oneToMany, one, removedMany);
            }

            {
                _manyToOne[many] = value;
                var manyValues = _oneToMany.GetValueOrDefault(value, []);
                Debug.Assert(!manyValues.Contains(many, _manyEquality));
                _oneToMany[value] = manyValues.Add(many);
            }
        }
    }

    public OneToMany()
    {
        _oneEquality = EqualityComparer<TOne>.Default;
        _manyEquality = EqualityComparer<TMany>.Default;
        _oneToMany = [];
        _manyToOne = [];
    }

    public OneToMany(
        IEqualityComparer<TOne>? oneEquality,
        IEqualityComparer<TMany>? manyEquality)
    {
        _oneEquality = oneEquality ?? EqualityComparer<TOne>.Default;
        _manyEquality = manyEquality ?? EqualityComparer<TMany>.Default;
        _oneToMany = new(_oneEquality);
        _manyToOne = new(_manyEquality);
    }

    public OneToMany(params ReadOnlySpan<(TOne one, TMany many)> mappings) : this()
    {
        AddRange(mappings);
    }

    public void Clear()
    {
        _oneToMany.Clear();
        _manyToOne.Clear();
    }

    public void Add(TOne one, TMany many)
    {
        if (!TryAdd(one, many))
            throw new ArgumentException("TMany value already mapped.");
    }

    public void AddRange(IEnumerable<(TOne one, TMany many)> mappings)
    {
        foreach (var (one, many) in mappings)
            Add(one, many);
    }

    public void AddRange(params ReadOnlySpan<(TOne one, TMany many)> mappings)
    {
        foreach (var (one, many) in mappings)
            Add(one, many);
    }

    public bool TryAdd(TOne one, TMany many)
    {
        if (!_manyToOne.TryAdd(many, one))
            return false;
        var manyValues = _oneToMany.GetValueOrDefault(one, []);
        Debug.Assert(!manyValues.Contains(many, _manyEquality));
        _oneToMany[one] = manyValues.Add(many);
        return true;
    }

    public bool TryRemove(TMany many, [MaybeNullWhen(false)] out TOne one)
    {
        if (!_manyToOne.Remove(many, out one))
            return false;
        var manyValues = _oneToMany[one];
        var removedMany = manyValues.Remove(many, _manyEquality);
        Debug.Assert(removedMany.Length < manyValues.Length);
        Many.SetOrRemove(_oneToMany, one, removedMany);
        return true;
    }

    public bool TryRemove(TMany many) => TryRemove(many, out _);

    public ImmutableArray<TMany> RemoveMany(TOne one)
    {
        if (!_oneToMany.TryGetValue(one, out var many))
            return [];
        foreach (var item in many)
        {
            var removedResult = _manyToOne.Remove(item, out var oldOne);
            Debug.Assert(removedResult);
            Debug.Assert(_oneEquality.Equals(one, oldOne));
        }
        return many;
    }

    public bool TryGetOne(TMany many, [MaybeNullWhen(false)] out TOne one) => _manyToOne.TryGetValue(many, out one);

    public ImmutableArray<TMany> GetMany(TOne one) => _oneToMany.GetValueOrDefault(one, []);
    public IEnumerable<(TOne one, TMany many)> EnumerateMappings() => _manyToOne.Select(pair => (pair.Value, pair.Key));
}
