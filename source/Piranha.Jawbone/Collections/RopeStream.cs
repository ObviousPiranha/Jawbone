using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Piranha.Jawbone;

public sealed class RopeStream : Stream
{
    private readonly ArrayPool<byte> _arrayPool;
    private readonly int _pageSize;
    private Segment[] _segments;
    private int _currentIndex;
    private long _position;
    private long _length;

    public long Capacity { get; private set; }
    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => true;
    public override long Length => _length;
    public override long Position
    {
        get => _position;
        set
        {
            if (value == 0)
            {
                _currentIndex = 0;
                _position = 0;
                return;
            }

            ArgumentOutOfRangeException.ThrowIfNegative(value);
            ArgumentOutOfRangeException.ThrowIfLessThan(_length, value, nameof(value));

            if (value < GetCurrentSegment().Offset)
            {
                do
                {
                    --_currentIndex;
                }
                while (value < GetCurrentSegment().Offset);
            }
            else if (GetCurrentSegment().NextOffset < value)
            {
                do
                {
                    ++_currentIndex;
                }
                while (GetCurrentSegment().NextOffset < value);
            }

            _position = value;
        }
    }

    public override bool CanTimeout => false;

    public RopeStream(
        int pageSize = 1 << 14,
        ArrayPool<byte>? arrayPool = default)
    {
        _pageSize = pageSize;
        _arrayPool = arrayPool ?? ArrayPool<byte>.Shared;
        _segments = new Segment[8];
        _segments.AsSpan().Fill(Segment.Empty);
    }

    public override void CopyTo(Stream destination, int bufferSize)
    {
        while (_position < _length)
        {
            if (_position == GetCurrentSegment().NextOffset)
                ++_currentIndex;
            var slice = GetCurrentSegment().GetArraySegment(_position);
            var size = Min(_length - _position, slice.Count);
            destination.Write(slice.Array!, slice.Offset, size);
            _position += size;
        }
    }

    public override async Task CopyToAsync(
        Stream destination,
        int bufferSize,
        CancellationToken cancellationToken)
    {
        while (_position < _length)
        {
            if (_position == GetCurrentSegment().NextOffset)
                ++_currentIndex;
            var slice = GetCurrentSegment().GetArraySegment(_position);
            var size = Min(_length - _position, slice.Count);
            await destination.WriteAsync(slice[..size], cancellationToken);
            _position += size;
        }
    }

    public override void Flush()
    {
    }

    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        return cancellationToken.IsCancellationRequested ?
            Task.FromCanceled(cancellationToken) :
            Task.CompletedTask;
    }

    public override int Read(byte[] buffer, int offset, int count) => Read(buffer.AsSpan(offset, count));

    public override int Read(Span<byte> buffer)
    {
        var available = _length - _position;
        var result = Min(available, buffer.Length);
        var remaining = buffer[..result];

        while (!remaining.IsEmpty)
        {
            if (_position == GetCurrentSegment().NextOffset)
                ++_currentIndex;
            var slice = GetCurrentSegment().Slice(_position);
            var size = int.Min(remaining.Length, slice.Length);
            slice[..size].CopyTo(remaining);
            remaining = remaining[size..];
            _position += size;
        }

        return result;
    }

    public override Task<int> ReadAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(Read(buffer, offset, count));
    }

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(Read(buffer.Span));
    }

    public override int ReadByte()
    {
        var b = default(byte);
        var n = Read(new Span<byte>(ref b));
        return n == 1 ? b : -1;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                Position = offset;
                break;
            case SeekOrigin.Current:
                Position += offset;
                break;
            case SeekOrigin.End:
                Position = _length + offset;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(origin));
        }

        return _position;
    }

    public override void SetLength(long value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);

        if (value < _position)
        {
            Position = value;
        }
        else if (_length < value)
        {
            EnsureAvailability();
            var position = _position;
            var index = _currentIndex;

            while (position < value)
            {
                if (_segments[index].NextOffset == position)
                    index = GetNext(index);
                var slice = _segments[index].Slice(position);
                var size = Min(value - position, slice.Length);
                slice[..size].Clear();
                position += size;
            }
        }

        _length = value;
    }

    public override string? ToString()
    {
        return base.ToString();
    }

    public override void Write(byte[] buffer, int offset, int count) => Write(buffer.AsSpan(offset, count));

    public override void Write(ReadOnlySpan<byte> buffer)
    {
        var remaining = buffer;

        EnsureAvailability();

        while (!remaining.IsEmpty)
        {
            if (GetCurrentSegment().NextOffset == _position)
                _currentIndex = GetNext(_currentIndex);
            var slice = GetCurrentSegment().Slice(_position);
            var size = int.Min(remaining.Length, slice.Length);
            remaining[..size].CopyTo(slice);
            remaining = remaining[size..];
            _position += size;
        }

        _length = long.Max(_length, _position);
    }

    public override Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
    {
        Write(buffer, offset, count);
        return Task.CompletedTask;
    }

    public override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        Write(buffer.Span);
        return ValueTask.CompletedTask;
    }

    public override void WriteByte(byte value)
    {
        Write(new ReadOnlySpan<byte>(in value));
    }

    protected override void Dispose(bool disposing)
    {
        foreach (ref var segment in _segments.AsSpan())
        {
            if (0 < segment.Buffer.Length)
            {
                _arrayPool.Return(segment.Buffer);
                segment = Segment.Empty;
            }
            else
            {
                break;
            }
        }

        _length = 0;
        _position = 0;
    }

    private ref Segment GetCurrentSegment() => ref _segments[_currentIndex];

    private byte[] Rent()
    {
        return _arrayPool.Rent(_pageSize);
    }

    private int GetNext(int index)
    {
        var result = index + 1;

        if (result == _segments.Length)
        {
            Array.Resize(ref _segments, _segments.Length * 2);
            _segments.AsSpan(result).Fill(Segment.Empty);
        }

        if (_segments[result].Buffer.Length == 0)
        {
            var buffer = Rent();
            _segments[result] = new Segment
            {
                Buffer = buffer,
                Offset = _segments[index].NextOffset
            };
            Capacity += buffer.Length;
        }

        return result;
    }

    private void EnsureAvailability()
    {
        if (_segments[0].Buffer.Length == 0)
        {
            _segments[0] = new Segment { Buffer = Rent() };
            Capacity = _segments[0].Buffer.Length;
        }
    }

    private static int Min(long i64, int i32)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(i64);
        ArgumentOutOfRangeException.ThrowIfNegative(i32);
        return unchecked(i64 < i32 ? (int)i64 : i32);
    }

    private readonly struct Segment
    {
        public required readonly byte[] Buffer { get; init; }
        public readonly long Offset { get; init; }
        public readonly long NextOffset => Offset + Buffer.Length;

        public readonly Span<byte> Slice(long startPosition)
        {
            var offset = GetIndex(startPosition);
            return Buffer.AsSpan(offset);
        }

        public readonly ArraySegment<byte> GetArraySegment(long startPosition)
        {
            var offset = GetIndex(startPosition);
            return new ArraySegment<byte>(Buffer, offset, Buffer.Length - offset);
        }

        private readonly int GetIndex(long startPosition) => (int)(startPosition - Offset);

        public static Segment Empty => new() { Buffer = [] };
    }
}
