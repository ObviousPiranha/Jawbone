using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone;

public static class StatisticalSummary
{
    public static StatisticalSummary<T> Create<T>(
        Func<T, float> toFloat,
        Func<float, T> fromFloat,
        List<T>? values)
    {
        return Create(
            toFloat,
            fromFloat,
            CollectionsMarshal.AsSpan(values));
    }

    public static StatisticalSummary<T> Create<T>(
        Func<T, float> toFloat,
        Func<float, T> fromFloat,
        params ReadOnlySpan<T> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(values[0], fromFloat.Invoke(0f));

        var pool = ArrayPool<float>.Shared;
        var array = pool.Rent(values.Length);

        try
        {
            for (int i = 0; i < values.Length; ++i)
                array[i] = toFloat.Invoke(values[i]);
            var summary = Calculate(array.AsSpan(0, values.Length));
            var result = summary.Select(fromFloat);
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<T> Create<T>(
        Func<T, double> toDouble,
        Func<double, T> fromDouble,
        List<T>? values)
    {
        return Create(
            toDouble,
            fromDouble,
            CollectionsMarshal.AsSpan(values));
    }

    public static StatisticalSummary<T> Create<T>(
        Func<T, double> toDouble,
        Func<double, T> fromDouble,
        params ReadOnlySpan<T> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(values[0], fromDouble.Invoke(0d));

        var pool = ArrayPool<double>.Shared;
        var array = pool.Rent(values.Length);

        try
        {
            for (int i = 0; i < values.Length; ++i)
                array[i] = toDouble.Invoke(values[i]);
            var summary = Calculate(array.AsSpan(0, values.Length));
            var result = summary.Select(fromDouble);
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<TimeSpan> Create(params ReadOnlySpan<TimeSpan> values)
    {
        return Create(
            static ts => ts.TotalMilliseconds,
            static d => TimeSpan.FromMilliseconds(d),
            values);
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
            var result = Calculate(array.AsSpan(0, values.Length));
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<float> Create(LoopyList<float> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Count == 1)
            return CreateMono(values[0]);

        var pool = ArrayPool<float>.Shared;
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
            var result = Calculate(array.AsSpan(0, values.Length));
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalSummary<double> Create(LoopyList<double> values)
    {
        if (values.IsEmpty)
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
    private static StatisticalSummary<T> CreateMono<T>(T n, T zero) => new(1, n, n, n, n, zero);

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
