using System;

namespace Jawbone.Test;

public class SpanReaderTests
{
    [Fact]
    public void TryGetSpan_String_Succeeds()
    {
        var s = "Hello";
        Assert.True(SpanReader.TryGetSpan(s, out var span));
        Assert.Equal(s.AsSpan(), span);
    }
}
