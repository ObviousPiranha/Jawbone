using System;
using System.IO;
using System.Text;

namespace Jawbone.Test;

public class CsvReaderTests
{
    [Fact]
    public void Internals_BufferResizesForLargeRow()
    {
        using var stream = new MemoryStream();
        var columns = new string[4];
        {
            using var writer = new StreamWriter(
                stream, leaveOpen: true);
            for (int i = 0; i < columns.Length; ++i)
            {
                if (0 < i)
                    writer.Write(',');
                columns[i] = new string((char)('a' + i), 1024);
                writer.Write(columns[i]);
            }
        }
        stream.Position = 0;
        var reader = new CsvReader(stream);
        Assert.Equal(columns, reader.ColumnNames);
    }

    [Fact]
    public void ReadsOneThousandRows()
    {
        var random = Random.Shared;
        var expectedColumnCount = 8;
        var expectedRowCount = 1000;
        using var stream = new MemoryStream(1 << 20);
        {
            using var writer = new StreamWriter(
                stream, leaveOpen: true);
            for (int i = 0; i < expectedColumnCount; ++i)
            {
                if (0 < i)
                    writer.Write(',');
                writer.Write("Column");
                writer.Write(i);
            }
            writer.Write('\n');

            for (int i = 0; i < expectedRowCount; ++i)
            {
                for (int j = 0; j < expectedColumnCount; ++j)
                {
                    if (0 < j)
                        writer.Write(',');
                    writer.Write(random.Next() - random.Next());
                }
                writer.Write('\n');
            }
        }
        stream.Position = 0;
        var reader = new CsvReader(stream);
        Assert.Equal(expectedColumnCount, reader.ColumnCount);

        for (int i = 0; i < expectedColumnCount; ++i)
            Assert.StartsWith("Column", reader.ColumnNames[i]);

        var actualRowCount = 0;
        while (reader.TryReadRow())
        {
            ++actualRowCount;
            Assert.Equal(expectedColumnCount, reader.FieldCount);

            for (int i = 0; i < reader.FieldCount; ++i)
                Assert.True(int.TryParse(reader.GetFieldUtf8(i), out _));
        }
        Assert.Equal(expectedRowCount, actualRowCount);
    }
}