using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;

namespace Piranha.Jawbone;

public sealed class RopeStream : Stream
{
    private readonly ArrayPool<byte> _arrayPool;
    private readonly int _pageSize;
    private long _length;
    private long _position;
    private Segment? _first;
    private Segment? _current;

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
                _current = _first;
                _position = 0;
                return;
            }

            ArgumentOutOfRangeException.ThrowIfNegative(value);
            ArgumentOutOfRangeException.ThrowIfLessThan(_length, value, nameof(value));
            if (_current is null)
                throw new InvalidOperationException("Current segment is missing!");

            if (value < _current.RunningIndex)
            {
                do
                {
                    _current = _current.PreviousSegment ?? throw new NullReferenceException("Missing previous segment!");
                }
                while (value < _current.RunningIndex);
            }
            else if (_current.NextIndex < value)
            {
                do
                {
                    _current = _current.NextSegment ?? throw new NullReferenceException("Missing next segment!");
                }
                while (_current.NextIndex < value);
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
    }

    public override void CopyTo(Stream destination, int bufferSize)
    {
        var buffer = _arrayPool.Rent(bufferSize);

        try
        {
            while (true)
            {
                var n = Read(buffer);
                if (n == 0)
                    return;
                destination.Write(buffer.AsSpan(0, n));
            }
        }
        finally
        {
            _arrayPool.Return(buffer);
        }
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count) => Read(buffer.AsSpan(offset, count));

    public override int Read(Span<byte> buffer)
    {
        var available = _length - _position;
        if (_current is null)
        {
            Debug.Assert(available == 0);
            return 0;
        }

        var result = Min(available, buffer.Length);
        var remaining = buffer[..result];

        while (!remaining.IsEmpty)
        {
            if (_position == _current.NextIndex)
                _current = _current.NextSegment ?? throw new NullReferenceException("Next segment cannot be null!");
            var size = int.Min(remaining.Length, _current.Remaining(_position));
            _current.Read(_position, remaining[..size]);
            remaining = remaining[size..];
            _position += size;
        }

        return result;
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
        if (value < _position)
        {
            Position = value;
        }
        else if (_length < value)
        {
            var remaining = value - _length;

            var segment = EnsureCurrent();
            var position = _position;

            while (0 < remaining)
            {
                if (segment.NextIndex == position)
                    segment = GetNext(segment);
                var size = Min(remaining, segment.Available(position));
                segment.Zero(position, size);
                remaining -= size;
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

        _current = EnsureCurrent();

        while (!remaining.IsEmpty)
        {
            if (_current.NextIndex == _position)
                _current = GetNext(_current);
            var size = int.Min(remaining.Length, _current.Available(_position));
            _current.Write(_position, remaining[..size]);
            remaining = remaining[size..];
            _position += size;
        }

        _length = long.Max(_length, _position);
    }

    public override void WriteByte(byte value)
    {
        base.WriteByte(value);
    }

    protected override void Dispose(bool disposing)
    {
        for (var segment = _first; segment is not null; segment = segment.NextSegment)
            _arrayPool.Return(segment.Buffer);

        _first = null;
        _current = null;
        _length = 0;
        _position = 0;
    }

    private byte[] Rent()
    {
        return _arrayPool.Rent(_pageSize);
    }

    private Segment GetNext(Segment segment)
    {
        var result = segment.NextSegment;

        if (result is null)
        {
            result = new Segment(segment.RunningIndex + segment.Buffer.Length, Rent(), segment);
            segment.NextSegment = result;
            Capacity += result.Buffer.Length;
        }

        return result;
    }

    private Segment EnsureCurrent()
    {
        if (_current is null)
        {
            Debug.Assert(_first is null);
            _first = new Segment(0, Rent());
            _current = _first;
            Capacity = _first.Buffer.Length;
        }

        return _current;
    }

    private static int Min(long i64, int i32)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(i64);
        ArgumentOutOfRangeException.ThrowIfNegative(i32);
        return unchecked(i64 < i32 ? (int)i64 : i32);
    }

    private sealed class Segment : ReadOnlySequenceSegment<byte>
    {
        public byte[] Buffer { get; }
        public Segment? NextSegment
        {
            get => (Segment?)Next;
            set => Next = value;
        }

        public Segment? PreviousSegment { get; }

        public long NextIndex => RunningIndex + Memory.Length;

        public Segment(long runningIndex, byte[] buffer, Segment? previousSegment = null)
        {
            RunningIndex = runningIndex;
            Buffer = buffer;
            Memory = buffer;
            PreviousSegment = previousSegment;
        }

        public int Available(long startPosition)
        {
            var offset = GetOffset(startPosition);
            return Buffer.Length - offset;
        }

        public void Write(long startPosition, ReadOnlySpan<byte> bytes)
        {
            var offset = GetOffset(startPosition);
            bytes.CopyTo(Buffer.AsSpan(offset));
        }

        public void Zero(long startPosition, int length)
        {
            var offset = GetOffset(startPosition);
            Buffer.AsSpan(offset, length).Clear();
        }

        public void Read(long startPosition, Span<byte> bytes)
        {
            var offset = GetOffset(startPosition);
            Buffer.AsSpan(offset, bytes.Length).CopyTo(bytes);
        }

        public int Remaining(long startPosition) => (int)(NextIndex - startPosition);

        private int GetOffset(long startPosition) => checked((int)(startPosition - RunningIndex));
    }
}
