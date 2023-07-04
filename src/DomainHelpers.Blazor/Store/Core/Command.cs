using System.Text.Json.Serialization;

namespace DomainHelpers.Blazor.Store.Core;

public abstract record Command {
    public record ForceReplaced(object State) : Command;

    public record Restored : Command;

    public record StateHasChanged(
        object State,
        object? Command = null,
        [property: JsonIgnore] Type? StoreType = null
    ) : Command {
        public override string Type => $"{StoreType?.Name ?? "Store"}+{GetType().Name}";
    }

    public record StateHasChanged<TCommand>(
        object State,
        TCommand? Command = default,
        [property: JsonIgnore] Type? StoreType = null
    ) : Command
        where TCommand : notnull {
        public override string Type => $"{StoreType?.Name ?? "Store"}+{GetType().Name}";
    }

    public virtual string Type {
        get {
            var type = GetType();
            return type.FullName?.Replace(
                type.Assembly.GetName().Name + ".",
                string.Empty
            ) ?? "";
        }
    }

    public string? GetFullTypeName() {
        return GetType().FullName;
    }

    public Dictionary<string, object> Payload => GetPayloads()
        .ToDictionary(x => x.Key, x => x.Value);

    IEnumerable<KeyValuePair<string, object>> GetPayloads() {
        foreach (var property in GetType().GetProperties()) {
            if (property.Name is nameof(Payload) or nameof(Type) or "StoreType") {
                continue;
            }

            var value = property.GetValue(this);
            if (value is not null) {
                yield return new KeyValuePair<string, object>(property.Name, value);
            }
        }
    }
}