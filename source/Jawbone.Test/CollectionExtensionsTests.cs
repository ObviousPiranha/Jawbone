using Jawbone.Extensions;
using System;
using System.Collections.Immutable;

namespace Jawbone.Test;

public class CollectionExtensionsTests
{
    public interface ITestable
    {
        void Test();
    }

    public record EnumerableIndexData<T>(
        ImmutableArray<T> Data,
        T Needle,
        ImmutableArray<int> ExpectedIndices) : ITestable
    {
        public void Test()
        {
            TestIndexEnumerable(
                Data.AsSpan(),
                Needle,
                ExpectedIndices.AsSpan());
        }
    }
    
    private static EnumerableIndexData<char> StringData(
        ReadOnlySpan<char> data,
        char needle,
        params ImmutableArray<int> expectedIndices)
    {
        return new(ImmutableArray.Create(data), needle, expectedIndices);
    }

    public static ITestable[] CreateTestables() =>
    [
        StringData("greetings", 'e', 2, 3),
        StringData("greetings", 'x'),
        StringData("   ", ' ', 0, 1, 2),
        new EnumerableIndexData<int>([1, 2, 3, 1, 2, 3], 1, [0, 3]),
        new EnumerableIndexData<int>([1, 2, 3, 1, 2, 3], 3, [2, 5]),
        new EnumerableIndexData<int>([1, 2, 3, 1, 2, 3], 5, [])
    ];
    
    public static TheoryData<ITestable> EnumerableIndexDataTheoryData => new(CreateTestables());

    [Fact]
    public void IndexEnumerable_MissingValueEnumeratesSafely()
    {
        foreach (var item in default(IndexEnumerable<int>))
            Assert.Fail("There should be no items here!");
        
        foreach (var item in "hello".EnumerateIndicesOf('x'))
            Assert.Fail("There should be no items here!");
    }

    [Theory]
    [MemberData(nameof(EnumerableIndexDataTheoryData))]
    public void IndexEnumerable_ValuesEnumeratedCorrectly(ITestable testable)
    {
        testable.Test();
    }

    private static void TestIndexEnumerable<T>(
        ReadOnlySpan<T> span,
        T needle,
        params ReadOnlySpan<int> expectedIndices)
    {
        var n = 0;
        foreach (var index in span.EnumerateIndicesOf(needle))
            Assert.Equal(expectedIndices[n++], index);
        Assert.Equal(expectedIndices.Length, n);
    }
}