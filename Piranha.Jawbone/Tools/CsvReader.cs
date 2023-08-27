using System;

namespace Piranha.Jawbone;

public delegate int ByteReader(Span<byte> buffer);

public sealed class CsvReader
{
    private readonly ByteReader _byteReader;
    private byte[] _data = new byte[256];
    private int _activeByteCount = 0;
    private int _currentRowBegin = 0;
    private int _currentRowLength = 0;
    private int _nextRowBegin = 0;

    public CsvReader(ByteReader byteReader)
    {
        _byteReader = byteReader;
    }

    private void RemoveCarriageReturn()
    {
        if (0 < _currentRowLength && _data[_currentRowBegin + _currentRowLength - 1] == '\r')
            --_currentRowLength;
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
                RemoveCarriageReturn();
                return true;
            }
            else if (_activeByteCount < _data.Length)
            {
                if (_nextRowBegin < _activeByteCount)
                {
                    _currentRowBegin = _nextRowBegin;
                    _currentRowLength = _activeByteCount - _currentRowBegin;
                    _nextRowBegin = _activeByteCount;
                    RemoveCarriageReturn();
                    return true;
                }
                else
                {
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
                RemoveCarriageReturn();
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
                RemoveCarriageReturn();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
