using Piranha.Jawbone.Net;
using Xunit;

namespace Piranha.Jawbone.Test;

public class StaticDataTest
{
    [Fact]
    public void IdMatchesIndex()
    {
        for (int i = 0; i < Linux.ErrorCodes.Length; ++i)
            Assert.Equal(i, Linux.ErrorCodes[i].Id);
    }
}
