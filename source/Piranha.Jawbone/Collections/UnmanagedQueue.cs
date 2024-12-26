using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Piranha.Jawbone;

file static class UnmanagedQueueExtensions
{
    public static Span<byte> Write<T>(
        this Span<byte> destination,
        T source
        ) where T : unmanaged
    {
        MemoryMarshal.Write(destination, source);
        return destination[Unsafe.SizeOf<T>()..];
    }

    public static ReadOnlySpan<byte> Read<T>(
        this ReadOnlySpan<byte> source,
        out T destination
        ) where T : unmanaged
    {
        destination = MemoryMarshal.Read<T>(source);
        return source[Unsafe.SizeOf<T>()..];
    }
}

public sealed class UnmanagedQueue
{
    private readonly Lock _lock = new();
    private readonly Dictionary<Type, int> _blobHandlerIndicesByType = [];
    private readonly List<BlobHandler> _blobHandlers = [];
    private byte[] _bytes = [];
    private int _begin = 0;
    private int _length = 0;

    public int AvailableBytes => _bytes.Length - _length;

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
            var afterIndex = bytes.Read(out int index);
            var handler = _blobHandlers[index];
            handler.Handle(afterIndex[..handler.Size]);
            var sizeOfBlobWithHeader = Unsafe.SizeOf<int>() + handler.Size;
            _length -= sizeOfBlobWithHeader;
            _begin = _length == 0 ? 0 : (_begin + sizeOfBlobWithHeader) % _bytes.Length;
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
        var end = _length == 0 ? 0 : (_begin + _length) % _bytes.Length;

        if (available < size)
        {
            var bytes = new byte[Math.Max((_length + size) * 4, size * 16)];

            if (0 < _length)
            {
                if (_begin < end)
                {
                    _bytes.AsSpan(_begin, _length).CopyTo(bytes);
                }
                else
                {
                    _bytes.AsSpan(_begin).CopyTo(bytes);
                    _bytes.AsSpan(0, end).CopyTo(bytes.AsSpan(_bytes.Length - _begin));
                }
            }

            _bytes = bytes;
            _begin = 0;
            end = _length;
        }
        else if (_begin < end)
        {
            var availableBytesAtEnd = _bytes.Length - end;

            if (availableBytesAtEnd < size)
            {
                // Move to the far end of the array to minimize
                // the amount of memory overlap.
                if (_begin < availableBytesAtEnd)
                {
                    var begin = _bytes.Length - _length;
                    _bytes.AsSpan(_begin, _length).CopyTo(_bytes.AsSpan(begin));
                    _begin = begin;
                    end = 0;
                }
                else
                {
                    _bytes.AsSpan(_begin, _length).CopyTo(_bytes);
                    _begin = 0;
                    end = _length;
                }
            }
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
            var item = MemoryMarshal.Read<T>(blob);
            _action.Invoke(item);
        }
    }
}
