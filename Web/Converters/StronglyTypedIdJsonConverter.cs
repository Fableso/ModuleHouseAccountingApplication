using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.StronglyTypedIds.Interface;

namespace Web.Converters;

public class StronglyTypedIdJsonConverter<T, TId> : JsonConverter<TId> where TId : IStronglyTypedId<T>
{
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonSerializer.Deserialize<T>(ref reader, options);

        var result = (TId?)Activator.CreateInstance(typeof(TId), value);
        
        if (result == null)
        {
            throw new JsonException($"Unable to create an instance of {typeof(TId)}.");
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Value, options);
    }
}