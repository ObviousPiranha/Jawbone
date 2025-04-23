using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone;

public static class StatisticalReport
{
    public static StatisticalReport<T> Create<T>(
        Func<T, float> toFloat,
        Func<float, T> fromFloat,
        List<T>? values)
    {
        return Create(
            toFloat,
            fromFloat,
            CollectionsMarshal.AsSpan(values));
    }

    public static StatisticalReport<T> Create<T>(
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
            var report = Calculate(array.AsSpan(0, values.Length));
            var result = report.Select(fromFloat);
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalReport<T> Create<T>(
        Func<T, double> toDouble,
        Func<double, T> fromDouble,
        List<T>? values)
    {
        return Create(
            toDouble,
            fromDouble,
            CollectionsMarshal.AsSpan(values));
    }

    public static StatisticalReport<T> Create<T>(
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
            var report = Calculate(array.AsSpan(0, values.Length));
            var result = report.Select(fromDouble);
            return result;
        }
        finally
        {
            pool.Return(array);
        }
    }

    public static StatisticalReport<TimeSpan> Create(params ReadOnlySpan<TimeSpan> values)
    {
        return Create(
            static ts => ts.TotalMilliseconds,
            static d => TimeSpan.FromMilliseconds(d),
            values);
    }

    public static StatisticalReport<float> CreateWithoutCopy(List<float>? values) => CreateWithoutCopy(CollectionsMarshal.AsSpan(values));
    public static StatisticalReport<float> CreateWithoutCopy(params Span<float> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(values[0]);

        return Calculate(values);
    }

    public static StatisticalReport<float> Create(List<float>? values) => Create(CollectionsMarshal.AsSpan(values));
    public static StatisticalReport<float> Create(params ReadOnlySpan<float> values)
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

    public static StatisticalReport<double> CreateWithoutCopy(List<double>? values) => CreateWithoutCopy(CollectionsMarshal.AsSpan(values));
    public static StatisticalReport<double> CreateWithoutCopy(params Span<double> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
            return CreateMono(values[0]);

        return Calculate(values);
    }

    public static StatisticalReport<double> Create(List<double>? values) => Create(CollectionsMarshal.AsSpan(values));
    public static StatisticalReport<double> Create(params ReadOnlySpan<double> values)
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

    private static StatisticalReport<float> Calculate(Span<float> values)
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

        var result = new StatisticalReport<float>(
            values.Length,
            min,
            max,
            mean,
            median,
            sd);

        return result;
    }

    private static StatisticalReport<double> Calculate(Span<double> values)
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

        var result = new StatisticalReport<double>(
            values.Length,
            min,
            max,
            mean,
            median,
            sd);

        return result;
    }

    private static bool IsOdd(int n) => (n & 1) == 1;
    private static StatisticalReport<float> CreateMono(float n) => new(1, n, n, n, n, 0f);
    private static StatisticalReport<double> CreateMono(double n) => new(1, n, n, n, n, 0d);
    private static StatisticalReport<T> CreateMono<T>(T n, T zero) => new(1, n, n, n, n, zero);

    public static StringBuilder AppendReport<T, TState>(
        this StringBuilder builder,
        in StatisticalReport<T> report,
        TState state,
        Action<StringBuilder, TState, T> append)
    {
        var word = report.SampleCount == 1 ? "sample" : "samples";
        builder
            .Append(report.SampleCount)
            .Append(' ')
            .Append(word);

        append.Invoke(builder.Append(": median "), state, report.Median);
        append.Invoke(builder.Append(" mean "), state, report.Mean);
        append.Invoke(builder.Append(" stddev "), state, report.StandardDeviation);
        append.Invoke(builder.Append(" min "), state, report.Min);
        append.Invoke(builder.Append(" max "), state, report.Max);
        return builder;
    }

    public static StatisticalReport<TResult> Select<T, TResult>(
        in this StatisticalReport<T> statisticalReport,
        Func<T, TResult> selector)
    {
        return new StatisticalReport<TResult>(
            statisticalReport.SampleCount,
            selector.Invoke(statisticalReport.Min),
            selector.Invoke(statisticalReport.Max),
            selector.Invoke(statisticalReport.Mean),
            selector.Invoke(statisticalReport.Median),
            selector.Invoke(statisticalReport.StandardDeviation));
    }
}

public struct StatisticalReport<T>
{
    public int SampleCount;
    public T Min;
    public T Max;
    public T Mean;
    public T Median;
    public T StandardDeviation;

    public StatisticalReport(
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
