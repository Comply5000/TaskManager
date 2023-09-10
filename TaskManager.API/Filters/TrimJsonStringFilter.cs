using Newtonsoft.Json;

namespace TaskManager.API.Filters;

public sealed class TrimJsonStringFilter : JsonConverter<string>
{
    public override string? ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value is string value)
        {
            return value.Trim() != string.Empty ? value.Trim() : null;
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.Trim());
    }
}