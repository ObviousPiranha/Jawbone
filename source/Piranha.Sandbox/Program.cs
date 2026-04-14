using Jawbone;
using Jawbone.Sdl3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Piranha.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine(new KeyMapping(SdlScancode.Kp7, KeyModifier.Alt));
            Console.WriteLine(new KeyMapping(SdlScancode.A));
            Console.WriteLine(new KeyMapping(SdlScancode.A, KeyModifier.Shift));

            var mappings = new Dictionary<KeyMapping, string>
            {
                [new KeyMapping(SdlScancode.A, KeyModifier.Control)] = "Huzzah"
            };

            var mappingsJson = JsonSerializer.Serialize(mappings);

            var keyMapping = new KeyMapping(SdlScancode.Kp7, KeyModifier.Control | KeyModifier.Alt);
            var serialized = JsonSerializer.SerializeToUtf8Bytes(keyMapping);
            Console.WriteLine(Encoding.UTF8.GetString(serialized));
            var deserialized = JsonSerializer.Deserialize<KeyMapping?>(serialized);
            Console.WriteLine(deserialized.ToString());

            var nullJson = "null"u8;
            var danger = JsonSerializer.Deserialize<KeyMapping?>(nullJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }
}
