using Jawbone.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Jawbone;

public sealed class CsvReader
{
    private const byte NewLine = (byte)'\n';
    private const byte Comma = (byte)',';

    private readonly ValueStream<byte> _byteReader;
    private readonly Dictionary<string, int> _columnIndexByName = [];
    private readonly List<string> _columnNames = [];
    private readonly List<int> _dividers = [];
    private byte[] _buffer = new byte[2048];
    private int _bufferBegin = 0;
    private int _bufferEnd = 0;
    private int _nextRowBegin = 0;
    private int _rowEnd = 0;
    private bool _eof;

    public int FieldCount => _dividers.Count - 1;
    public int ColumnCount => _columnIndexByName.Count;
    public ReadOnlySpan<string> ColumnNames => CollectionsMarshal.AsSpan(_columnNames);

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
                    _columnNames.Add(name);
                }
            }
        }
    }

    public bool TryReadRow()
    {
        _bufferBegin = _nextRowBegin;

        _rowEnd = _buffer
            .AsSpan(0, _bufferEnd)
            .SkipAndIndexOf(_bufferBegin, NewLine);

        if (_rowEnd == -1 && !_eof)
        {
            if (0 < _bufferBegin)
            {
                _buffer
                    .AsSpan(_bufferBegin.._bufferEnd)
                    .CopyTo(_buffer);
                _bufferEnd -= _bufferBegin;
                _bufferBegin = 0;
            }

            do
            {
                if (_bufferEnd == _buffer.Length)
                    Array.Resize(ref _buffer, _buffer.Length * 2);
                var available = _buffer.Length - _bufferEnd;
                var n = _byteReader.Invoke(_buffer.AsSpan(_bufferEnd));
                _eof = n < available;
                var lineEnding = _buffer.AsSpan(_bufferEnd, n).IndexOf(NewLine);
                if (0 <= lineEnding)
                    _rowEnd = _bufferEnd + lineEnding;
                _bufferEnd += n;
            }
            while (_rowEnd == -1 && !_eof);
        }

        if (_rowEnd == -1)
            _rowEnd = _nextRowBegin = _bufferEnd;
        else
            _nextRowBegin = _rowEnd + 1;
        
        _dividers.Clear();
        if (_bufferBegin == _bufferEnd)
            return false;
        
        _dividers.Add(_bufferBegin - 1);
        var row = _buffer.AsSpan(_bufferBegin.._rowEnd);
        foreach (var index in row.EnumerateIndicesOf(Comma))
            _dividers.Add(_bufferBegin + index);
        _dividers.Add(_rowEnd);
        
        return true;
    }

    /// <summary>
    /// Gets span referencing the field contents as UTF-8.
    /// </summary>
    public ReadOnlySpan<byte> GetFieldUtf8(int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, FieldCount);
        var low = _dividers[index] + 1;
        var high = _dividers[index + 1];
        return _buffer.AsSpan(low..high);
    }

    /// <summary>
    /// Gets newly allocated string containing the field contents.
    /// </summary>
    public string GetFieldUtf16(int index) => Encoding.UTF8.GetString(GetFieldUtf8(index));

    public int GetIndex(string columnName) => _columnIndexByName[columnName];
    public bool TryGetIndex(string columnName, out int index) => _columnIndexByName.TryGetValue(columnName, out index);
    public bool HasColumn(string columnName) => _columnIndexByName.ContainsKey(columnName);
}
