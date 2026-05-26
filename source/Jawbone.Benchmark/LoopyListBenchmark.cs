using BenchmarkDotNet.Attributes;
using System.Collections.Generic;

namespace Jawbone.Benchmark;

[MemoryDiagnoser(false)]
public class LoopyListBenchmark
{
    private readonly LoopyList<int> _loopyList = new();
    private readonly List<int> _list = new();
    private readonly Queue<int> _queue = new();

    public LoopyListBenchmark()
    {
        for (int i = 0; i < 16; ++i)
        {
            _loopyList.PushBack(i);
            _list.Add(i);
            _queue.Enqueue(i);
        }
    }

    [Benchmark(Baseline = true)]
    public void LoopyListPopTwoPushTwo()
    {
        _ = _loopyList.PopFront();
        _ = _loopyList.PopFront();
        _loopyList.PushBack(99);
        _loopyList.PushBack(99);
    }

    [Benchmark]
    public void ListPopTwoPushTwo()
    {
        _list.RemoveAt(0);
        _list.RemoveAt(0);
        _list.Add(99);
        _list.Add(99);
    }

    [Benchmark]
    public void QueuePopTwoPushTwo()
    {
        _queue.Dequeue();
        _queue.Dequeue();
        _queue.Enqueue(99);
        _queue.Enqueue(99);
    }
}
