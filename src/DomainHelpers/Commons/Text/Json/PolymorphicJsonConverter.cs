using System.Text.Json;
using System.Text.Json.Serialization;

namespace DomainHelpers.Commons.Text.Json;

public class PolymorphicJsonConverter<T> : JsonConverter<T> {
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType is not JsonTokenType.StartObject) {
            throw new JsonException("Expected string");
        }

        IEnumerable<Type> allTypes = typeof(T).Assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t));

        foreach (Type type in allTypes) {
            if (IsFit(reader, type.GetProperties().Select(x => x.Name))) {
                return (T)JsonSerializer.Deserialize(ref reader, type)!;
            }
        }

        throw new JsonException("Invalid JSON");
    }

    private static bool IsFit(Utf8JsonReader reader, IEnumerable<string> tokens) {
        if (tokens.Count() is 0) {
            return false;
        }

        HashSet<string> set = new HashSet<string>();
        while (reader.Read()) {
            if (reader.TokenType is JsonTokenType.EndObject) {
                break;
            }

            if (reader.TokenType is JsonTokenType.PropertyName) {
                set.Add(reader.GetString() ?? "");
            }
        }

        foreach (string token in tokens) {
            if (set.Contains(token) is false) {
                return false;
            }
        }

        return true;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
        writer.WriteStringValue(JsonSerializer.Serialize(value));
    }
}