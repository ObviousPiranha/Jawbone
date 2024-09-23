using System;
using System.Security.Cryptography;
using Xunit;

namespace Piranha.Jawbone.Test;

public class RopeStreamTest
{
    [Fact]
    public void BasicTest()
    {
        var message = "HUZZAH"u8;
        var buffer = new byte[message.Length * 2];
        using var ropeStream = new RopeStream(64);
        ropeStream.Write(message);
        Assert.Equal(message.Length, ropeStream.Position);
        ropeStream.Position = 0;

        var n = ropeStream.Read(buffer);
        Assert.Equal(message.Length, n);
        Assert.Equal(message, buffer.AsSpan(0, n));

        ropeStream.SetLength(0);
        Assert.Equal(0, ropeStream.Length);
        Assert.Equal(0, ropeStream.Position);
    }

    [Fact]
    public void CrossPages()
    {
        var originalMessage = new byte[2048];
        originalMessage.AsSpan().Fill(111);
        using var ropeStream = new RopeStream(64);
        ropeStream.Write(originalMessage);

        var buffer = new byte[originalMessage.Length * 2];
        Assert.False(originalMessage.AsSpan().SequenceEqual(buffer.AsSpan(0, originalMessage.Length)));
        ropeStream.Position = 0;

        var n = ropeStream.Read(buffer);
        Assert.Equal(originalMessage.Length, n);
        Assert.Equal(originalMessage.AsSpan(), buffer.AsSpan(0, n));
        Assert.Equal(n, ropeStream.Position);

        ropeStream.Position /= 2;
    }

    [Fact]
    public void BenchmarkPrep()
    {
        var buffer = new byte[9000];
        RandomNumberGenerator.Fill(buffer);

        using var ropeStream = new RopeStream();

        for (int i = 0; i < 100; ++i)
            ropeStream.Write(buffer);

        Assert.Equal(9000 * 100, ropeStream.Length);
    }
}
