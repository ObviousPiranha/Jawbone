using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
                var manyValues =_oneToMany.GetValueOrDefault(value, []);
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
        _oneToMany = new(oneEquality);
        _manyToOne = new(manyEquality);
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

    public bool TryRemove(TMany many)
    {
        if (!_manyToOne.Remove(many, out var one))
            return false;
        var manyValues = _oneToMany[one];
        var removedMany = manyValues.Remove(many, _manyEquality);
        Debug.Assert(removedMany.Length < manyValues.Length);
        Many.SetOrRemove(_oneToMany, one, removedMany);
        return true;
    }

    public ImmutableArray<TMany> GetMany(TOne one) => _oneToMany.GetValueOrDefault(one, []);
    public IEnumerable<(TOne one, TMany many)> EnumerateMappings() => _manyToOne.Select(pair => (pair.Value, pair.Key));
}