using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jawbone;

[StructLayout(LayoutKind.Sequential)]
public struct Rectangle32 : IEquatable<Rectangle32>
{
    public Point32 Position;
    public Point32 Size;

    public Rectangle32(Point32 position, Point32 size)
    {
        Position = position;
        Size = size;
    }

    public override readonly bool Equals(object? obj) => obj is Rectangle32 r && Equals(r);
    public override readonly int GetHashCode() => HashCode.Combine(Position.X, Position.Y, Size.X, Size.Y);
    public override readonly string ToString() => $"position {Position} size {Size.X}x{Size.Y}";
    public readonly bool Equals(Rectangle32 other) => Position.Equals(other.Position) && Size.Equals(other.Size);

    public static Rectangle32 FromCorners(Point32 low, Point32 high) => new(low, high - low);

    public static bool operator ==(Rectangle32 a, Rectangle32 b) => a.Equals(b);
    public static bool operator !=(Rectangle32 a, Rectangle32 b) => !a.Equals(b);

    public sealed class SimpleJsonConverter : JsonConverter<Rectangle32>
    {
        public override Rectangle32 Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var text = reader.GetString() ?? throw new JsonException();
            var values = text.Split(",", StringSplitOptions.TrimEntries);
            var result = new Rectangle32(
                new(int.Parse(values[0]), int.Parse(values[1])),
                new(int.Parse(values[2]), int.Parse(values[3])));
            return result;
        }

        public override void Write(
            Utf8JsonWriter writer,
            Rectangle32 value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(
                $"{value.Position.X}, {value.Position.Y}, {value.Size.X}, {value.Size.Y}");
        }
    }
}
