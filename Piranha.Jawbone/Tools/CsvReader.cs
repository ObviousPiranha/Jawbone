using System;

namespace Piranha.Jawbone;

public delegate int ByteReader(Span<byte> buffer);

public sealed class CsvReader
{
    private readonly ByteReader _byteReader;
    private byte[] _row = new byte[256];
    private int _used = 0;
    private int _currentRowBegin = 0;
    private int _currentRowLength = 0;
    private int _nextRowIndex = 0;

    public CsvReader(ByteReader byteReader)
    {
        _byteReader = byteReader;
    }

    private void RemoveCarriageReturn()
    {
        if (0 < _currentRowLength && _row[_currentRowBegin + _currentRowLength - 1] == '\r')
            --_currentRowLength;
    }

    public bool TryReadRow()
    {
        // First see if the next entire row is in the buffer already.
        if (_nextRowIndex < _used)
        {
            var nextRow = _row.AsSpan(_nextRowIndex.._used);
            var indexOfNewline = nextRow.IndexOf((byte)'\n');

            if (0 <= indexOfNewline)
            {
                _currentRowBegin = _nextRowIndex;
                _currentRowLength = indexOfNewline;
                RemoveCarriageReturn();
                _nextRowIndex += indexOfNewline + 1;

                return true;
            }

            // Slide data left in preparation for more data.
            nextRow.CopyTo(_row);
            _used -= _nextRowIndex;
        }
        else
        {
            _used = 0;
        }

        _currentRowBegin = 0;
        _currentRowLength = 0;

        while (true)
        {
            int newDataIndex = _used;
            _used = _byteReader.Invoke(_row.AsSpan(_used));

            var indexOfNewline = _row.AsSpan(newDataIndex.._used).IndexOf((byte)'\n');
            if (0 <= indexOfNewline)
            {
                _currentRowLength = newDataIndex + indexOfNewline;
                _nextRowIndex = _currentRowLength + 1;
                RemoveCarriageReturn();
                return true;
            }
            else if (_used == _row.Length)
            {
                Array.Resize(ref _row, _row.Length * 2);
            }
            else if (0 < _used)
            {
                _nextRowIndex = _used;
                _currentRowLength = _used;
                RemoveCarriageReturn();
                return true;
            }
            else
            {
                _nextRowIndex = 0;
                return false;
            }
        }
    }
}
