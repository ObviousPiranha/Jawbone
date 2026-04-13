using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Jawbone;

public sealed class ManyToMany<T0, T1>
    where T0 : notnull
    where T1 : notnull
{
    private readonly IEqualityComparer<T0>? _leftEquality;
    private readonly IEqualityComparer<T1>? _rightEquality;
    private readonly Dictionary<T0, ImmutableArray<T1>> _leftToRight;
    private readonly Dictionary<T1, ImmutableArray<T0>> _rightToLeft;

    public ManyToMany()
    {
        _leftToRight = [];
        _rightToLeft = [];
    }

    public ManyToMany(
        IEqualityComparer<T0>? leftEquality,
        IEqualityComparer<T1>? rightEquality)
    {
        _leftEquality = leftEquality;
        _rightEquality = rightEquality;
        _leftToRight = new(leftEquality);
        _rightToLeft = new(rightEquality);
    }

    public bool TryAdd(T0 leftValue, T1 rightValue)
    {
        if (_leftToRight.TryGetValue(leftValue, out var rightValues))
        {
            Debug.Assert(!rightValues.IsDefaultOrEmpty);
            if (rightValues.Contains(rightValue, _rightEquality))
                return false;
            _leftToRight[leftValue] = rightValues.Add(rightValue);
        }
        else
        {
            _leftToRight.Add(leftValue, rightValues = [rightValue]);
        }

        if (_rightToLeft.TryGetValue(rightValue, out var leftValues))
        {
            Debug.Assert(!leftValues.IsDefaultOrEmpty);
            _rightToLeft[rightValue] = leftValues.Add(leftValue);
        }
        else
        {
            _rightToLeft.Add(rightValue, [leftValue]);
        }

        return true;
    }

    public bool TryRemove(T0 leftValue, T1 rightValue)
    {
        if (!_leftToRight.TryGetValue(leftValue, out var rightValues))
            return false;
        Debug.Assert(!rightValues.IsDefaultOrEmpty);
        var rightRemoved = rightValues.Remove(rightValue, _rightEquality);
        if (rightRemoved.Length == rightValues.Length)
            return false;
        if (rightRemoved.IsEmpty)
            _leftToRight.Remove(leftValue);
        else
            _leftToRight[leftValue] = rightRemoved;
        
        var leftValues = _rightToLeft[rightValue];
        Debug.Assert(!leftValues.IsDefaultOrEmpty);
        var leftRemoved = leftValues.Remove(leftValue, _leftEquality);
        Debug.Assert(leftRemoved.Length < leftValues.Length);
        if (leftRemoved.IsEmpty)
            _rightToLeft.Remove(rightValue);
        else
            _rightToLeft[rightValue] = leftRemoved;
        return true;
    }

    public ImmutableArray<T0> GetLeftValues() => _leftToRight.Keys.ToImmutableArray();
    public ImmutableArray<T0> GetLeftValues(T1 rightValue) => _rightToLeft.GetValueOrDefault(rightValue, []);
    public ImmutableArray<T1> GetRightValues() => _rightToLeft.Keys.ToImmutableArray();
    public ImmutableArray<T1> GetRightValues(T0 leftValue) => _leftToRight.GetValueOrDefault(leftValue, []);

    public IEnumerable<(T0 left, T1 right)> EnumerateMappings()
    {
        return _leftToRight.SelectMany(pair => pair.Value.Select(r => (pair.Key, r)));
    }
}
