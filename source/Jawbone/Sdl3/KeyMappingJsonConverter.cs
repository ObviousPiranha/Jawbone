using Jawbone.Sdl3;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jawbone;

public sealed class KeyMappingJsonConverter : JsonConverter<KeyMapping>
{
    private ReadOnlySpan<byte> Ctrl => "Ctrl"u8;
    private ReadOnlySpan<byte> Shift => "Shift"u8;
    private ReadOnlySpan<byte> Alt => "Alt"u8;
    private ReadOnlySpan<byte> Super => "Super"u8;
    public override KeyMapping Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException();
        var spanReader = SpanReader.Create(reader.ValueSpan);
        var mod = default(KeyModifier);
        if (spanReader.TryMatch(Ctrl))
            mod |= KeyModifier.Control;
        if (spanReader.TryMatch(Shift))
            mod |= KeyModifier.Shift;
        if (spanReader.TryMatch(Alt))
            mod |= KeyModifier.Alt;
        if (spanReader.TryMatch(Super))
            mod |= KeyModifier.Super;
        var id = int.Parse(spanReader.Pending);
        var scancode = (SdlScancode)id;
        var result = new KeyMapping(scancode, mod);
        return result;
    }

    public override void Write(
        Utf8JsonWriter writer,
        KeyMapping value,
        JsonSerializerOptions options)
    {
        var spanWriter = SpanWriter.Create(stackalloc byte[32]);
        if (value.Modifier.MaskAll(KeyModifier.Control))
            spanWriter.Write(Ctrl);
        if (value.Modifier.MaskAll(KeyModifier.Shift))
            spanWriter.Write(Shift);
        if (value.Modifier.MaskAll(KeyModifier.Alt))
            spanWriter.Write(Alt);
        if (value.Modifier.MaskAll(KeyModifier.Super))
            spanWriter.Write(Super);
        if (!spanWriter.TryFormat((int)value.Scancode))
            throw new JsonException();
        writer.WriteStringValue(spanWriter.Written);

    }
}