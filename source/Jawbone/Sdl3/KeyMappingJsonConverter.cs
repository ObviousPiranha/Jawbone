using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jawbone.Sdl3;

public sealed class KeyMappingJsonConverter : JsonConverter<KeyMapping>
{
    public override KeyMapping Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException();
        var result = KeyMapping.ParseUtf8Json(reader.ValueSpan);
        return result;
    }

    public override void Write(
        Utf8JsonWriter writer,
        KeyMapping value,
        JsonSerializerOptions options)
    {
        var spanWriter = SpanWriter.Create(stackalloc byte[32]);
        if (!value.TryWriteUtf8Json(ref spanWriter))
            throw new JsonException();
        writer.WriteStringValue(spanWriter.Written);

    }
}