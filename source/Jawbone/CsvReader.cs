using Jawbone.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jawbone;

public sealed class CsvReader
{
    private readonly ValueStream<byte> _byteReader;
    private readonly Dictionary<string, int> _columnIndexByName = [];
    private readonly List<int> _dividers = [];
    private byte[] _buffer = new byte[2048];
    private int _byteCount = 0;
    private int _nextRow = 0;
    private int _rowEnd = 0;
    private bool _eof;

    public int FieldCount => _dividers.Count - 1;

    public ReadOnlySpan<byte> this[int index] => GetFieldUtf8(index);

    public CsvReader(Stream stream) : this(stream.Read)
    {
    }

    public CsvReader(ValueStream<byte> byteReader)
    {
        _byteReader = byteReader;
        if (TryReadRow())
        {
            for (int i = 0; i < FieldCount; ++i)
            {
                var data = GetFieldUtf8(i);
                if (!data.IsEmpty)
                {
                    var name = Encoding.UTF8.GetString(data);
                    _columnIndexByName.Add(name, i);
                }
            }
        }
    }

    public bool TryReadRow()
    {
        _buffer.AsSpan(_nextRow.._byteCount).CopyTo(_buffer);
        _byteCount -= _nextRow;

        _rowEnd = _buffer.AsSpan(0, _byteCount).IndexOf((byte)'\n');

        while (_rowEnd == -1 && !_eof)
        {
            if (_byteCount == _buffer.Length)
                Array.Resize(ref _buffer, _buffer.Length * 2);
            var available = _buffer.Length - _byteCount;
            var n = _byteReader.Invoke(_buffer.AsSpan(_byteCount));
            _eof = n < available;
            var lineEnding = _buffer.AsSpan(_byteCount, n).IndexOf((byte)'\n');
            if (0 <= lineEnding)
                _rowEnd = _byteCount + lineEnding;
            _byteCount += n;
        }

        if (_rowEnd == -1)
            _rowEnd = _nextRow = _byteCount;
        else
            _nextRow = _rowEnd + 1;
        
        if (_nextRow == 0)
            return false;
        
        _dividers.Clear();
        _dividers.Add(-1);
        var row = _buffer.AsSpan(0, _rowEnd);
        foreach (var index in row.EnumerateIndicesOf((byte)','))
            _dividers.Add(index);
        _dividers.Add(_rowEnd);
        
        return true;
    }

    public ReadOnlySpan<byte> GetFieldUtf8(int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, FieldCount);
        var low = _dividers[index] + 1;
        var high = _dividers[index + 1];
        return _buffer.AsSpan(low..high);
    }

    public string GetFieldUtf16(int index) => Encoding.UTF8.GetString(GetFieldUtf8(index));

    public string[] GetColumnNames() => _columnIndexByName.Keys.ToArray();
}
