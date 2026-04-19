using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Jawbone;

public sealed class OneToOne<T0, T1> : IDictionary<T0, T1>
    where T0 : notnull
    where T1 : notnull
{
    private readonly Dictionary<T0, T1> _leftToRight;
    private readonly Dictionary<T1, T0> _rightToLeft;

    public OneToOne<T1, T0> Reversed { get; }

    ICollection<T0> IDictionary<T0, T1>.Keys => _leftToRight.Keys;
    ICollection<T1> IDictionary<T0, T1>.Values => _leftToRight.Values;
    bool ICollection<KeyValuePair<T0, T1>>.IsReadOnly => false;
    public int Count => _leftToRight.Count;

    public T1 this[T0 key]
    {
        get => _leftToRight[key];
        set
        {
            if (_leftToRight.Remove(key, out var oldRight))
            {
                var removeResult = _rightToLeft.Remove(oldRight, out var previousKey);
                Debug.Assert(removeResult);
                Debug.Assert(_leftToRight.Comparer.Equals(key, previousKey));
            }

            if (_rightToLeft.Remove(value, out var oldLeft))
            {
                var removeResult = _leftToRight.Remove(oldLeft, out var previousValue);
                Debug.Assert(removeResult);
                Debug.Assert(_rightToLeft.Comparer.Equals(value, previousValue));
            }

            _leftToRight[key] = value;
            _rightToLeft[value] = key;
        }
    }

    public OneToOne()
    {
        _leftToRight = [];
        _rightToLeft = [];
        Reversed = new(this);
    }

    public OneToOne(
        IEqualityComparer<T0>? leftEquality,
        IEqualityComparer<T1>? rightEquality)
    {
        _leftToRight = new(leftEquality);
        _rightToLeft = new(rightEquality);
        Reversed = new(this);
    }

    private OneToOne(OneToOne<T1, T0> reversed)
    {
        _leftToRight = reversed._rightToLeft;
        _rightToLeft = reversed._leftToRight;
        Reversed = reversed;
    }

    public IDictionary<T0, T1> AsDictionary() => this;

    public bool TryAdd(T0 left, T1 right)
    {
        if (_leftToRight.TryAdd(left, right))
        {
            if (_rightToLeft.TryAdd(right, left))
                return true;
            else
                _leftToRight.Remove(left);
        }

        return false;
    }

    public bool TryRemoveLeft(T0 left, [MaybeNullWhen(false)] out T1 right)
    {
        if (_leftToRight.Remove(left, out right))
        {
            var removedResult = _rightToLeft.Remove(right, out var oldLeft);
            Debug.Assert(removedResult);
            Debug.Assert(_leftToRight.Comparer.Equals(left, oldLeft));
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryRemoveLeft(T0 left) => TryRemoveLeft(left, out _);

    public bool TryRemoveRight(T1 right, [MaybeNullWhen(false)] out T0 left)
    {
        if (_rightToLeft.Remove(right, out left))
        {
            var removedResult = _leftToRight.Remove(left, out var oldRight);
            Debug.Assert(removedResult);
            Debug.Assert(_rightToLeft.Comparer.Equals(right, oldRight));
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryRemoveRight(T1 right) => TryRemoveRight(right, out _);

    public bool TryGetRight(T0 left, [MaybeNullWhen(false)] out T1 right) => _leftToRight.TryGetValue(left, out right);
    public bool TryGetLeft(T1 right, [MaybeNullWhen(false)] out T0 left) => _rightToLeft.TryGetValue(right, out left);

    void IDictionary<T0, T1>.Add(T0 key, T1 value)
    {
        _leftToRight.Add(key, value);

        try
        {
            _rightToLeft.Add(value, key);
        }
        catch
        {
            _leftToRight.Remove(key);
            throw;
        }
    }

    bool IDictionary<T0, T1>.ContainsKey(T0 key) => _leftToRight.ContainsKey(key);

    bool IDictionary<T0, T1>.Remove(T0 key)
    {
        if (_leftToRight.Remove(key, out var value))
        {
            var removedResult = _rightToLeft.Remove(value, out var oldKey);
            Debug.Assert(removedResult);
            Debug.Assert(_leftToRight.Comparer.Equals(key, oldKey));
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IDictionary<T0, T1>.TryGetValue(T0 key, [MaybeNullWhen(false)] out T1 value) => _leftToRight.TryGetValue(key, out value);
    void ICollection<KeyValuePair<T0, T1>>.Add(KeyValuePair<T0, T1> item) => AsDictionary().Add(item.Key, item.Value);

    public void Clear()
    {
        _leftToRight.Clear();
        _rightToLeft.Clear();
    }

    bool ICollection<KeyValuePair<T0, T1>>.Contains(KeyValuePair<T0, T1> item) => AsDictionary().Contains(item);
    void ICollection<KeyValuePair<T0, T1>>.CopyTo(KeyValuePair<T0, T1>[] array, int arrayIndex) => AsDictionary().CopyTo(array, arrayIndex);
    bool ICollection<KeyValuePair<T0, T1>>.Remove(KeyValuePair<T0, T1> item)
    {
        var result =
            _leftToRight.TryGetValue(item.Key, out var value) &&
            _rightToLeft.Comparer.Equals(value, item.Value) &&
            _leftToRight.Remove(item.Key) &&
            _rightToLeft.Remove(item.Value);
        return result;
    }

    IEnumerator<KeyValuePair<T0, T1>> IEnumerable<KeyValuePair<T0, T1>>.GetEnumerator() => AsDictionary().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => AsDictionary().GetEnumerator();
}
