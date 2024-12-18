using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TechnicalAxos_OscarBarrera.Helpers.Json;

public class StringToLongArrayConverter : JsonConverter<long[]>
{
    public override long[] ReadJson(JsonReader reader, Type objectType, long[] existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token.Type == JTokenType.Array)
        {
            return token
                .Select(item =>
                {
                    if (long.TryParse(item.ToString(), out var result))
                        return result;
                    return (long?)null; // Handle invalid values
                })
                .Where(v => v.HasValue)
                .Select(v => v.Value)
                .ToArray();
        }

        return Array.Empty<long>();
    }

    public override void WriteJson(JsonWriter writer, long[] value, JsonSerializer serializer)
    {
        JArray array = new JArray(value);
        array.WriteTo(writer);
    }
}
