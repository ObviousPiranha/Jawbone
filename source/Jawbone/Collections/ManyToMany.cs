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

    public ManyToMany<T1, T0> Reversed { get; }

    public ManyToMany()
    {
        _leftToRight = [];
        _rightToLeft = [];
        Reversed = new(this);
    }

    public ManyToMany(
        IEqualityComparer<T0>? leftEquality,
        IEqualityComparer<T1>? rightEquality)
    {
        _leftEquality = leftEquality;
        _rightEquality = rightEquality;
        _leftToRight = new(leftEquality);
        _rightToLeft = new(rightEquality);
        Reversed = new(this);
    }

    private ManyToMany(ManyToMany<T1, T0> reversed)
    {
        _leftEquality = reversed._rightEquality;
        _rightEquality = reversed._leftEquality;
        _leftToRight = reversed._rightToLeft;
        _rightToLeft = reversed._leftToRight;
        Reversed = reversed;
    }

    public void Clear()
    {
        _leftToRight.Clear();
        _rightToLeft.Clear();
    }

    public bool TryAdd(T0 left, T1 right)
    {
        if (_leftToRight.TryGetValue(left, out var rightValues))
        {
            Debug.Assert(!rightValues.IsDefaultOrEmpty);
            if (rightValues.Contains(right, _rightEquality))
                return false;
            _leftToRight[left] = rightValues.Add(right);
        }
        else
        {
            _leftToRight.Add(left, [right]);
        }

        if (_rightToLeft.TryGetValue(right, out var leftValues))
        {
            Debug.Assert(!leftValues.IsDefaultOrEmpty);
            Debug.Assert(!leftValues.Contains(left, _leftEquality));
            _rightToLeft[right] = leftValues.Add(left);
        }
        else
        {
            _rightToLeft.Add(right, [left]);
        }

        return true;
    }

    public bool TryRemove(T0 left, T1 right)
    {
        if (!_leftToRight.TryGetValue(left, out var rightValues))
            return false;
        Debug.Assert(!rightValues.IsDefaultOrEmpty);
        var rightRemoved = rightValues.Remove(right, _rightEquality);
        if (rightRemoved.Length == rightValues.Length)
            return false;
        Many.SetOrRemove(_leftToRight, left, rightRemoved);

        var leftValues = _rightToLeft[right];
        Debug.Assert(!leftValues.IsDefaultOrEmpty);
        var leftRemoved = leftValues.Remove(left, _leftEquality);
        Debug.Assert(leftRemoved.Length < leftValues.Length);
        Many.SetOrRemove(_rightToLeft, right, leftRemoved);
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
