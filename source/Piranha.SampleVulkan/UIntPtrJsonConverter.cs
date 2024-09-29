using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Piranha.SampleVulkan;
internal class UIntPtrJsonConverter : JsonConverter<UIntPtr>
{
    public override nuint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (UIntPtr.Size)
        {
            case 4:
                return reader.GetUInt32();

            case 8:
                return (nuint)reader.GetUInt64();
        }
        return UIntPtr.Zero;
    }

    public override void Write(Utf8JsonWriter writer, nuint value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(value.ToString());
    }
}
