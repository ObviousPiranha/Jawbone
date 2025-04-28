using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone;

public static class StatisticalSummary
{
    public static StatisticalSummary<float> Create<T>(
        List<T>? values,
        Func<T, float> toFloat)
    {
        return Create(
            CollectionsMarshal.AsSpan(values),
            toFloat);
    }

    public static StatisticalSummary<float> Create<T>(
        ReadOnlySpan<T> values,
        Func<T, float> toFloat)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(toFloat.Invoke(values[0]));

        var pool = ArrayPool<float>.Shared;
        var array = pool.Rent(values.Length);

        try
        {
            for (int i = 0; i < values.Length; ++i)
                array[i] = toFloat.Invoke(values[i]);
            var result = Calculate(array.AsSpan(0, values.Length));
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<float> Create<T>(
        LoopyList<T>? values,
        Func<T, float> toFloat)
    {
        if (values is null || values.IsEmpty)
            return default;

        if (values.Count == 1)
            return CreateMono(toFloat.Invoke(values[0]));

        var pool = ArrayPool<float>.Shared;
        var array = pool.Rent(values.Count);

        try
        {
            for (int i = 0; i < values.Count; ++i)
                array[i] = toFloat.Invoke(values[i]);
            var result = Calculate(array.AsSpan(0, values.Count));
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<float> Create<T>(
        LoopyList<T> values,
        Range range,
        Func<T, float> toFloat)
    {
        var (start, count) = range.GetOffsetAndLength(values.Count);

        if (count == 0)
            return default;

        if (count == 1)
            return CreateMono(toFloat.Invoke(values[start]));

        var pool = ArrayPool<float>.Shared;
        var array = pool.Rent(count);

        try
        {
            for (int i = 0; i < count; ++i)
                array[i] = toFloat.Invoke(values[start + 1]);
            var result = Calculate(array.AsSpan(0, count));
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<double> Create<T>(
        List<T>? values,
        Func<T, double> toDouble)
    {
        return Create(
            CollectionsMarshal.AsSpan(values),
            toDouble);
    }

    public static StatisticalSummary<double> Create<T>(
        ReadOnlySpan<T> values,
        Func<T, double> toDouble)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(toDouble.Invoke(values[0]));

        var pool = ArrayPool<double>.Shared;
        var array = pool.Rent(values.Length);

        try
        {
            for (int i = 0; i < values.Length; ++i)
                array[i] = toDouble.Invoke(values[i]);
            var result = Calculate(array.AsSpan(0, values.Length));
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<double> Create<T>(
        LoopyList<T>? values,
        Func<T, double> toDouble)
    {
        if (values is null || values.IsEmpty)
            return default;

        if (values.Count == 1)
            return CreateMono(toDouble.Invoke(values[0]));

        var pool = ArrayPool<double>.Shared;
        var array = pool.Rent(values.Count);

        try
        {
            for (int i = 0; i < values.Count; ++i)
                array[i] = toDouble.Invoke(values[i]);
            var result = Calculate(array.AsSpan(0, values.Count));
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<double> Create<T>(
        LoopyList<T> values,
        Range range,
        Func<T, double> toDouble)
    {
        var (start, count) = range.GetOffsetAndLength(values.Count);

        if (count == 0)
            return default;

        if (count == 1)
            return CreateMono(toDouble.Invoke(values[0]));

        var pool = ArrayPool<double>.Shared;
        var array = pool.Rent(count);

        try
        {
            for (int i = 0; i < count; ++i)
                array[i] = toDouble.Invoke(values[start + i]);
            var result = Calculate(array.AsSpan(0, count));
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<TimeSpan> Create(List<TimeSpan>? values)
    {
        return Create(CollectionsMarshal.AsSpan(values));
    }

    public static StatisticalSummary<TimeSpan> Create(params ReadOnlySpan<TimeSpan> values)
    {
        return Create(values, static ts => ts.TotalMilliseconds)
            .Select(static sample => TimeSpan.FromMilliseconds(sample));
    }

    public static StatisticalSummary<TimeSpan> Create(LoopyList<TimeSpan>? values)
    {
        return Create(values, static ts => ts.TotalMilliseconds)
            .Select(static sample => TimeSpan.FromMilliseconds(sample));
    }

    public static StatisticalSummary<float> CreateWithoutCopy(List<float>? values) => CreateWithoutCopy(CollectionsMarshal.AsSpan(values));
    public static StatisticalSummary<float> CreateWithoutCopy(params Span<float> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(values[0]);

        return Calculate(values);
    }

    public static StatisticalSummary<float> Create(List<float>? values) => Create(CollectionsMarshal.AsSpan(values));
    public static StatisticalSummary<float> Create(params ReadOnlySpan<float> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(values[0]);

        var pool = ArrayPool<float>.Shared;
        var array = pool.Rent(values.Length);

        try
        {
            values.CopyTo(array);
            var span = array.AsSpan(0, values.Length);
            var result = Calculate(span);
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<float> Create(LoopyList<float>? values)
    {
        if (values is null || values.IsEmpty)
            return default;

        if (values.Count == 1)
            return CreateMono(values[0]);

        var pool = ArrayPool<float>.Shared;
        var array = pool.Rent(values.Count);

        try
        {
            values.CopyTo(array);
            var span = array.AsSpan(0, values.Count);
            var result = Calculate(span);
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<float> Create(LoopyList<float> values, Range range)
    {
        var (start, count) = range.GetOffsetAndLength(values.Count);

        if (count == 0)
            return default;

        if (count == 1)
            return CreateMono(values[start]);

        var pool = ArrayPool<float>.Shared;
        var array = pool.Rent(count);

        try
        {
            values.CopyTo(range, array);
            var span = array.AsSpan(0, count);
            var result = Calculate(span);
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<double> CreateWithoutCopy(List<double>? values) => CreateWithoutCopy(CollectionsMarshal.AsSpan(values));
    public static StatisticalSummary<double> CreateWithoutCopy(params Span<double> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(values[0]);

        return Calculate(values);
    }

    public static StatisticalSummary<double> Create(List<double>? values) => Create(CollectionsMarshal.AsSpan(values));
    public static StatisticalSummary<double> Create(params ReadOnlySpan<double> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(values[0]);

        var pool = ArrayPool<double>.Shared;
        var array = pool.Rent(values.Length);

        try
        {
            values.CopyTo(array);
            var span = array.AsSpan(0, values.Length);
            var result = Calculate(span);
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<double> Create(LoopyList<double>? values)
    {
        if (values is null || values.IsEmpty)
            return default;

        if (values.Count == 1)
            return CreateMono(values[0]);

        var pool = ArrayPool<double>.Shared;
        var array = pool.Rent(values.Count);

        try
        {
            values.CopyTo(array);
            var span = array.AsSpan(0, values.Count);
            var result = Calculate(array.AsSpan(0, values.Count));
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<double> Create(LoopyList<double> values, Range range)
    {
        var (start, count) = range.GetOffsetAndLength(values.Count);

        if (count == 0)
            return default;

        if (count == 1)
            return CreateMono(values[start]);

        var pool = ArrayPool<double>.Shared;
        var array = pool.Rent(count);

        try
        {
            values.CopyTo(range, array);
            var span = array.AsSpan(0, count);
            var result = Calculate(span);
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    private static StatisticalSummary<float> Calculate(Span<float> values)
    {
        values.Sort();
        var min = values[0];
        var max = values[^1];
        float median;

        if (IsOdd(values.Length))
        {
            median = values[values.Length / 2];
        }
        else
        {
            int index = values.Length / 2;
            var high = values[index];
            var low = values[index - 1];
            median = (low + high) / 2f;
        }

        var sum = values[0];
        for (int i = 1; i < values.Length; ++i)
            sum += values[i];
        var mean = sum / values.Length;

        // https://stackoverflow.com/a/3141731
        var sdSum = 0f;
        foreach (var value in values)
            sdSum += float.Pow(value - mean, 2f);

        var sd = float.Sqrt(sdSum / (values.Length - 1));

        var result = new StatisticalSummary<float>(
            values.Length,
            min,
            max,
            mean,
            median,
            sd);

        return result;
    }

    private static StatisticalSummary<double> Calculate(Span<double> values)
    {
        values.Sort();
        var min = values[0];
        var max = values[^1];
        double median;

        if (IsOdd(values.Length))
        {
            median = values[values.Length / 2];
        }
        else
        {
            int index = values.Length / 2;
            var high = values[index];
            var low = values[index - 1];
            median = (low + high) / 2d;
        }

        var sum = values[0];
        for (int i = 1; i < values.Length; ++i)
            sum += values[i];
        var mean = sum / values.Length;

        // https://stackoverflow.com/a/3141731
        var sdSum = 0d;
        foreach (var value in values)
            sdSum += double.Pow(value - mean, 2d);

        var sd = double.Sqrt(sdSum / (values.Length - 1));

        var result = new StatisticalSummary<double>(
            values.Length,
            min,
            max,
            mean,
            median,
            sd);

        return result;
    }

    private static bool IsOdd(int n) => (n & 1) == 1;
    private static StatisticalSummary<float> CreateMono(float n) => new(1, n, n, n, n, 0f);
    private static StatisticalSummary<double> CreateMono(double n) => new(1, n, n, n, n, 0d);

    public static StringBuilder AppendSummary<T, TState>(
        this StringBuilder builder,
        in StatisticalSummary<T> summary,
        TState state,
        Action<StringBuilder, TState, T> append)
    {
        var word = summary.SampleCount == 1 ? "sample" : "samples";
        builder
            .Append(summary.SampleCount)
            .Append(' ')
            .Append(word);

        append.Invoke(builder.Append(": median "), state, summary.Median);
        append.Invoke(builder.Append(" mean "), state, summary.Mean);
        append.Invoke(builder.Append(" stddev "), state, summary.StandardDeviation);
        append.Invoke(builder.Append(" min "), state, summary.Min);
        append.Invoke(builder.Append(" max "), state, summary.Max);
        return builder;
    }

    public static StatisticalSummary<TResult> Select<T, TResult>(
        in this StatisticalSummary<T> summary,
        Func<T, TResult> selector)
    {
        return new StatisticalSummary<TResult>(
            summary.SampleCount,
            selector.Invoke(summary.Min),
            selector.Invoke(summary.Max),
            selector.Invoke(summary.Mean),
            selector.Invoke(summary.Median),
            selector.Invoke(summary.StandardDeviation));
    }
}

public struct StatisticalSummary<T>
{
    public int SampleCount;
    public T Min;
    public T Max;
    public T Mean;
    public T Median;
    public T StandardDeviation;

    public StatisticalSummary(
        int sampleCount,
        T min,
        T max,
        T mean,
        T median,
        T standardDeviation)
    {
        SampleCount = sampleCount;
        Min = min;
        Max = max;
        Mean = mean;
        Median = median;
        StandardDeviation = standardDeviation;
    }

    public readonly string ToString(Func<T, string?> converter)
    {
        var word = SampleCount == 1 ? "sample" : "samples";

        var result = string.Concat(
            SampleCount.ToString(),
            " ",
            word,
            ": median ",
            converter.Invoke(Median),
            " mean ",
            converter.Invoke(Mean),
            " stddev ",
            converter.Invoke(StandardDeviation),
            " min ",
            converter.Invoke(Min),
            " max ",
            converter.Invoke(Max));

        return result;
    }

    public override readonly string ToString() => ToString(static v => v?.ToString());
}
