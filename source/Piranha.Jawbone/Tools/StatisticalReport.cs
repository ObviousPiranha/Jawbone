using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Jawbone;

public static class StatisticalReport
{
    public static StatisticalReport<TOutput> Convert<TInput, TOutput>(
        this StatisticalReport<TInput> sr,
        Converter<TInput, TOutput> converter)
    {
        return new StatisticalReport<TOutput>(
            sr.SampleCount,
            converter(sr.Min),
            converter(sr.Max),
            converter(sr.Mean),
            converter(sr.Median),
            converter(sr.StandardDeviation));
    }

    public static StatisticalReport<T> Create<T>(
        Converter<T, double> toDouble,
        Converter<double, T> fromDouble,
        params T[] values)
    {
        var doubles = Array.ConvertAll(values, toDouble);
        return Create(doubles).Convert(fromDouble);
    }

    public static StatisticalReport<T> Create<T>(
        Converter<T, double> toDouble,
        Converter<double, T> fromDouble,
        List<T> values)
    {
        var doubles = values.ConvertAll(toDouble);
        return Create(doubles).Convert(fromDouble);
    }

    public static StatisticalReport<double> Create(List<double>? values) => Create(CollectionsMarshal.AsSpan(values));
    public static StatisticalReport<double> Create(params Span<double> values)
    {
        if (values.IsEmpty)
            return default;

        if (values.Length == 1)
        {
            var n = values[0];
            return new StatisticalReport<double>(1, n, n, n, n, default);
        }

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

        var standardDeviation = double.Sqrt(sdSum / (values.Length - 1));

        return new StatisticalReport<double>(
            values.Length,
            min,
            max,
            mean,
            median,
            standardDeviation);
    }

    public static StatisticalReport<double> CreateAndClear(List<double> values)
    {
        var result = Create(values);
        values.Clear();
        return result;
    }

    private static bool IsOdd(int n) => (n & 1) == 1;

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
}

public readonly struct StatisticalReport<T>
{
    public readonly int SampleCount { get; }
    public readonly T Min { get; }
    public readonly T Max { get; }
    public readonly T Mean { get; }
    public readonly T Median { get; }
    public readonly T StandardDeviation { get; }

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

    public string ToString(Func<T, string?> converter)
    {
        return ToString(converter, (n, state) => state.Invoke(n));
    }

    public string ToString<TState>(TState state, Func<T, TState, string?> converter)
    {
        var word = SampleCount == 1 ? "sample" : "samples";

        var result = string.Concat(
            SampleCount.ToString(),
            " ",
            word,
            ": median ",
            converter.Invoke(Median, state),
            " mean ",
            converter.Invoke(Mean, state),
            " stddev ",
            converter.Invoke(StandardDeviation, state),
            " min ",
            converter.Invoke(Min, state),
            " max ",
            converter.Invoke(Max, state));

        return result;
    }

    public override string ToString() => ToString(v => v is null ? string.Empty : v.ToString());
}
