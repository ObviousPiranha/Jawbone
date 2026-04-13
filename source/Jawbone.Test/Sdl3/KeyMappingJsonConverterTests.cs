using System;
using System.Text;
using System.Text.Json;

namespace Jawbone.Sdl3.Test;

public class KeyMappingJsonConverterTests
{
    public static TheoryData<SdlScancode, KeyModifier, string> KeyMappingTheoryData =>
    [
        new(SdlScancode.A, KeyModifier.None, "4"),
        new(SdlScancode.A, KeyModifier.Control, "Ctrl4"),
        new(SdlScancode.A, KeyModifier.Alt | KeyModifier.Control, "CtrlAlt4")
    ];

    [Theory]
    [MemberData(nameof(KeyMappingTheoryData))]
    public void KeyMappingJsonRoundTrip(
        SdlScancode scancode,
        KeyModifier modifier,
        string expectedJson)
    {
        var expectedKeyMapping = new KeyMapping(scancode, modifier);
        var json = JsonSerializer.SerializeToUtf8Bytes(expectedKeyMapping);
        var actualJson = Encoding.UTF8.GetString(json.AsSpan(1..^1));
        var actualKeyMapping = JsonSerializer.Deserialize<KeyMapping>(json);
        Assert.Equal(expectedJson, actualJson);
        Assert.Equal(expectedKeyMapping, actualKeyMapping);
    }
}