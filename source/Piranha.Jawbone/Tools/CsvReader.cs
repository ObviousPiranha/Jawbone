using System;

namespace Piranha.Jawbone;

public delegate int ByteReader(Span<byte> buffer);

public sealed class CsvReader
{
    private static ByteReader DefaultByteReader => static _ => 0;

    private ByteReader _byteReader = DefaultByteReader;
    private byte[] _data = new byte[256];
    private int[] _dividerIndices = new int[16];
    private int _activeByteCount = 0;
    private int _currentRowBegin = 0;
    private int _currentRowLength = 0;
    private int _nextRowBegin = 0;
    private int _fieldCount = 0;

    public ReadOnlySpan<byte> CurrentRow => _data.AsSpan(_currentRowBegin, _currentRowLength);
    public int FieldCount => _fieldCount;

    public ReadOnlySpan<byte> this[int index] => GetField(index);

    public CsvReader()
    {
        _dividerIndices[0] = -1;
    }

    public void Start(ByteReader byteReader)
    {
        _byteReader = byteReader;
        _fieldCount = 0;
        _activeByteCount = 0;
        _currentRowBegin = 0;
        _currentRowLength = 0;
        _nextRowBegin = 0;
    }

    private void PrepareRow()
    {
        if (0 < _currentRowLength && _data[_currentRowBegin + _currentRowLength - 1] == '\r')
            --_currentRowLength;

        var row = CurrentRow;
        _fieldCount = 0;
        int offset = 0;
        while (true)
        {
            if (++_fieldCount == _dividerIndices.Length)
                Array.Resize(ref _dividerIndices, _dividerIndices.Length * 2);

            var commaIndex = row[offset..].IndexOf((byte)',');

            if (0 <= commaIndex)
            {
                _dividerIndices[_fieldCount] = commaIndex + offset;
                offset += commaIndex + 1;
            }
            else
            {
                _dividerIndices[_fieldCount] = row.Length;
                break;
            }
        }
    }

    public bool TryReadRow()
    {
        if (0 < _nextRowBegin)
        {
            var nextRowData = _data.AsSpan(_nextRowBegin.._activeByteCount);
            var newLineIndex = nextRowData.IndexOf((byte)'\n');

            if (0 <= newLineIndex)
            {
                _currentRowBegin = _nextRowBegin;
                _currentRowLength = newLineIndex;
                _nextRowBegin += _currentRowLength + 1;
                PrepareRow();
                return true;
            }
            else if (_activeByteCount < _data.Length)
            {
                if (_nextRowBegin < _activeByteCount)
                {
                    _currentRowBegin = _nextRowBegin;
                    _currentRowLength = _activeByteCount - _currentRowBegin;
                    _nextRowBegin = _activeByteCount;
                    PrepareRow();
                    return true;
                }
                else
                {
                    _currentRowBegin = 0;
                    _currentRowLength = 0;
                    _fieldCount = 0;
                    return false;
                }
            }
            else
            {
                nextRowData.CopyTo(_data);
                _activeByteCount -= _nextRowBegin;
                _nextRowBegin = 0;
            }
        }

        _currentRowBegin = 0;
        _currentRowLength = 0;

        while (true)
        {
            var newDataIndex = _activeByteCount;
            _activeByteCount += _byteReader.Invoke(_data.AsSpan(newDataIndex));
            var newBytes = _data.AsSpan(newDataIndex.._activeByteCount);
            var newLineIndex = newBytes.IndexOf((byte)'\n');

            if (0 <= newLineIndex)
            {
                _currentRowLength = newDataIndex + newLineIndex;
                _nextRowBegin = _currentRowLength + 1;
                PrepareRow();
                return true;
            }
            else if (_activeByteCount == _data.Length)
            {
                Array.Resize(ref _data, _data.Length * 2);
            }
            else if (0 < _activeByteCount)
            {
                _currentRowLength = _activeByteCount;
                _nextRowBegin = _activeByteCount;
                PrepareRow();
                return true;
            }
            else
            {
                _fieldCount = 0;
                return false;
            }
        }
    }

    public ReadOnlySpan<byte> GetField(int index)
    {
        if (index < 0 || _fieldCount <= index)
            throw new ArgumentOutOfRangeException(nameof(index));

        var low = _dividerIndices[index] + 1;
        var high = _dividerIndices[index + 1];
        return CurrentRow[low..high];
    }

    public ReadOnlySpan<byte> GetFields(int index, int count)
    {
        if (index < 0 || _fieldCount <= index)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (count < 0 || _fieldCount < index + count)
            throw new ArgumentOutOfRangeException(nameof(count));

        var low = _dividerIndices[index] + 1;
        var high = _dividerIndices[index + count];
        return CurrentRow[low..high];
    }

    public ReadOnlySpan<byte> GetFields(int startIndex)
    {
        if (startIndex < 0 || _fieldCount < startIndex)
            throw new ArgumentOutOfRangeException(nameof(startIndex));

        if (startIndex == _fieldCount)
            return default;

        var low = _dividerIndices[startIndex] + 1;
        var high = _dividerIndices[_fieldCount];
        return CurrentRow[low..high];
    }
}
