using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeySiri.Core.SystemTextJson;

public class UnixDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long ts;
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                ts = reader.GetInt64();
                break;
            case JsonTokenType.String:
                ts = long.Parse(reader.GetString());
                break;
            default:
                throw new JsonException($"Could not parse {reader.TokenType} value to a DateTime");
        }

        return DateTimeOffset.FromUnixTimeSeconds(ts);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}