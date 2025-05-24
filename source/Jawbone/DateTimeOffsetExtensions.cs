using System;

namespace Jawbone.Extensions;

public static class DateTimeOffsetExtensions
{
    public static string ToFullString(this DateTimeOffset dto) => dto.ToString("s") + dto.ToString("zzz");
}
