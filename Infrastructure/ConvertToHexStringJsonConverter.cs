using System.Text.Json;
using System.Text.Json.Serialization;

internal class ConvertToHexStringJsonConverter : JsonConverter<byte[]>
{
    public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
      writer.WriteStringValue(Convert.ToHexString(value));
    }
}