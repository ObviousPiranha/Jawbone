using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

file static class UnmanagedQueueExtensions
{
    public static Span<byte> Write<T>(
        this Span<byte> destination,
        in T source
        ) where T : unmanaged
    {
        MemoryMarshal.AsBytes(
            new ReadOnlySpan<T>(source)).CopyTo(destination);
        return destination[Unsafe.SizeOf<T>()..];
    }

    public static ReadOnlySpan<byte> Read<T>(
        this ReadOnlySpan<byte> source,
        out T destination
        ) where T : unmanaged
    {
        Unsafe.SkipInit(out destination);
        source.CopyTo(
            MemoryMarshal.AsBytes(
                new Span<T>(ref destination)));
        return source[Unsafe.SizeOf<T>()..];
    }
}

// Assumes single producer and single consumer.
public sealed class UnmanagedQueue
{
    private readonly object _lock = new();
    private readonly Dictionary<Type, int> _blobHandlerIndicesByType = new();
    private readonly List<BlobHandler> _blobHandlers = new();
    private byte[] _bytes = Array.Empty<byte>();
    private int _begin = 0;
    private int _length = 0;

    private int GetEnd() => (_begin + _length) % _bytes.Length;

    public void Register<T>(Action<T> action) where T : unmanaged
    {
        var blobHandler = new BlobHandler<T>(action);

        lock (_lock)
        {
            if (_blobHandlerIndicesByType.TryGetValue(typeof(T), out var index))
            {
                _blobHandlers[index] = blobHandler;
            }
            else
            {
                index = _blobHandlers.Count;
                _blobHandlers.Add(blobHandler);
                _blobHandlerIndicesByType.Add(typeof(T), index);
            }
        }
    }

    public bool TryEnqueue<T>(T item) where T : unmanaged
    {
        lock (_lock)
        {
            if (!_blobHandlerIndicesByType.TryGetValue(typeof(T), out var index))
                return false;

            var bytes = Allocate(Unsafe.SizeOf<int>() + Unsafe.SizeOf<T>());
            bytes.Write(index).Write(item);
            _length += bytes.Length;
            return true;
        }
    }

    public bool TryDequeue()
    {
        lock (_lock)
        {
            if (_length == 0)
                return false;

            ReadOnlySpan<byte> bytes = _bytes.AsSpan(_begin);
            var blob = bytes.Read(out int index);
            var handler = _blobHandlers[index];
            handler.Handle(blob);
            var sizeOfBlobWithHeader = Unsafe.SizeOf<int>() + handler.Size;
            _begin = (_begin + sizeOfBlobWithHeader) % _bytes.Length;
            _length -= sizeOfBlobWithHeader;
            return true;
        }
    }

    public void DequeueAll()
    {
        while (TryDequeue())
            ;
    }

    private Span<byte> Allocate(int size)
    {
        var available = _bytes.Length - _length;
        var end = GetEnd();

        if (available < size)
        {
            var bytes = new byte[Math.Max((_length + size) * 4, size * 16)];

            if (_begin < end)
            {
                _bytes.AsSpan(_begin, _length).CopyTo(bytes);
            }
            else
            {
                _bytes.AsSpan(_begin).CopyTo(bytes);
                _bytes.AsSpan(0, end).CopyTo(bytes.AsSpan(_bytes.Length - _begin));
            }

            _bytes = bytes;
            _begin = 0;
            end = _length;
        }
        else if (_begin < end && (_bytes.Length - end) < size)
        {
            int offset = _bytes.Length - _begin;
            var span = _bytes.AsSpan();

            // Simple and really fast on bytes.
            span.Reverse();
            span[..offset].Reverse();
            span[offset..].Reverse();

            _begin = 0;
            end = _length;
        }

        var result = _bytes.AsSpan(end, size);
        _length += size;
        return result;
    }

    private abstract class BlobHandler
    {
        public int Size { get; protected init; }
        public abstract void Handle(ReadOnlySpan<byte> blob);
    }

    private sealed class BlobHandler<T> : BlobHandler where T : unmanaged
    {
        private readonly Action<T> _action;

        public BlobHandler(Action<T> action)
        {
            Size = Unsafe.SizeOf<T>();
            _action = action;
        }

        public override void Handle(ReadOnlySpan<byte> blob)
        {
            var item = default(T);
            var span = new Span<T>(ref item);
            var asBytes = MemoryMarshal.AsBytes(span);
            blob.CopyTo(asBytes);
            _action.Invoke(item);
        }
    }
}
