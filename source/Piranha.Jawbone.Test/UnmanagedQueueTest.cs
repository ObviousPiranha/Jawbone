using System;
using System.Numerics;

namespace Piranha.Jawbone.Test;

public class UnmanagedQueueTest
{
    [Fact]
    public void QueueCallsHandlers()
    {
        var intMessage = 1337;
        var matrixMessage = Matrix4x4.Identity;
        var dateTimeMessage = new DateTime(2000, 1, 1, 11, 30, 29);

        var intMessageWasHandled = false;
        var matrixMessageWasHandled = false;
        var dateTimeMessageWasHandled = false;

        var queue = new UnmanagedQueue();
        queue.Register<int>(item =>
        {
            Assert.Equal(intMessage, item);
            intMessageWasHandled = true;
        });
        queue.Register<Matrix4x4>(item =>
        {
            Assert.Equal(matrixMessage, item);
            matrixMessageWasHandled = true;
        });
        queue.Register<DateTime>(item =>
        {
            Assert.Equal(dateTimeMessage, item);
            dateTimeMessageWasHandled = true;
        });

        Assert.True(queue.TryEnqueue(intMessage));
        Assert.True(queue.TryEnqueue(matrixMessage));
        Assert.True(queue.TryEnqueue(dateTimeMessage));

        queue.DequeueAll();

        Assert.True(intMessageWasHandled);
        Assert.True(matrixMessageWasHandled);
        Assert.True(dateTimeMessageWasHandled);
    }

    [Fact]
    public void QueueWorks()
    {
        var queue = new UnmanagedQueue();
        queue.Register<TimeSpan>(static _ => { });
        queue.Register<byte>(static _ => { });

        for (int i = 0; i < 12; ++i)
            Assert.True(queue.TryEnqueue(TimeSpan.MaxValue));

        // Toss in a weird shape to prevent even fit.
        Assert.True(queue.TryEnqueue(byte.MaxValue));

        while (queue.TryDequeue() && queue.TryDequeue())
            Assert.True(queue.TryEnqueue(TimeSpan.MinValue));
    }

    [Fact]
    public void QueueResizeWorks()
    {
        const int N = unchecked((int)0xdeadbeef);
        var queue = new UnmanagedQueue();
        queue.Register<int>(static v => Assert.Equal(N, v));
        queue.Register<Matrix4x4>(static v => Matrix4x4.Identity.Equals(v));

        Assert.True(queue.TryEnqueue(N));

        while (0 < queue.AvailableBytes)
            Assert.True(queue.TryEnqueue(N));

        Assert.True(queue.TryDequeue());
        Assert.True(queue.TryDequeue());
        Assert.True(queue.TryEnqueue(N));
        Assert.True(queue.TryEnqueue(Matrix4x4.Identity));
        Assert.True(queue.TryEnqueue(Matrix4x4.Identity));
        queue.DequeueAll();
    }
}
