using System;
using System.Numerics;
using Xunit;

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
}
