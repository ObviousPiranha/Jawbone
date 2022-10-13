using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Piranha.Jawbone.Net;

namespace Piranha.Jawbone.Benchmark;

[MemoryDiagnoser(false)]
public class AddressKeyBenchmark<TAddress> where TAddress : unmanaged, IAddress<TAddress>
{
    private static Endpoint<TAddress> CreateRandom()
    {
        var result = default(Endpoint<TAddress>);

        unsafe
        {
            var span = new Span<byte>(&result, Unsafe.SizeOf<Endpoint<TAddress>>());
            Random.Shared.NextBytes(span);
        }

        return result;
    }

    private static void Throw() => throw new Exception("Kablam");

    private readonly List<Endpoint<TAddress>> _list = new();
    private readonly HashSet<Endpoint<TAddress>> _set = new();
    private readonly Dictionary<Endpoint<TAddress>, Endpoint<Address128>> _dictionary = new();
    private Endpoint<TAddress> _last;

    [Params(4, 8, 64)]
    public int Count { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _list.Clear();

        for (int i = 0; i < Count; ++i)
            _list.Add(CreateRandom());
        
        _set.Clear();
        _set.UnionWith(_list);

        _dictionary.Clear();
        
        foreach (var item in _list)
            _dictionary[item] = default;

        _last = _list.Last();
    }

    [Benchmark(Baseline = true)]
    public void FindItemInList()
    {
        foreach (var item in _list)
        {
            if (item.Equals(_last))
                return;
        }

        Throw();
    }

    [Benchmark]
    public void FindItemInSet()
    {
        if (!_set.Contains(_last))
            Throw();
    }

    [Benchmark]    
    public void FindItemInDictionary()
    {
        if (!_dictionary.ContainsKey(_last))
            Throw();
    }
}