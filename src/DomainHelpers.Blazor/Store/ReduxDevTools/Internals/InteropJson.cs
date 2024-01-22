using System.Text.Json;
using System.Text.Json.Serialization;

namespace DomainHelpers.Blazor.Store.ReduxDevTools.Internals;
public readonly record struct StoreAction {
    [JsonPropertyName("action")]
    public required ActionItem Action { get; init; }

    [JsonPropertyName("timestamp")]
    public required long Timestamp { get; init; }

    [JsonPropertyName("stack")]
    public string? Stack { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }
}

public readonly record struct ActionItemFromDevtool(
    string Type,
    JsonElement? Payload,
    string? Source
);

public record struct ActionItem(
    [property:JsonPropertyName("type")]
string? Type,
    [property:JsonPropertyName("payload")]
object? Payload,
    [property:JsonPropertyName("declaredType")]
string? DeclaredType,
    [property:JsonPropertyName("storeName")]
string StoreName
);

public readonly record struct HistoryStateContextJson {
    [JsonPropertyName("actionsById")]
    public required Dictionary<int, StoreAction> ActionsById { get; init; }

    [JsonPropertyName("computedStates")]
    public required ComputedState[] ComputedStates { get; init; }

    [JsonPropertyName("currentStateIndex")]
    public required int CurrentStateIndex { get; init; }

    [JsonPropertyName("nextActionId")]
    public required int NextActionId { get; init; }

    [JsonPropertyName("skippedActionIds")]
    public required int[] SkippedActionIds { get; init; }

    [JsonPropertyName("stagedActionIds")]
    public required int[] StagedActionIds { get; init; }

    [JsonPropertyName("isLocked")]
    public bool IsLocked { get; init; }

    [JsonPropertyName("isPaused")]
    public bool IsPaused { get; init; }
}

public readonly record struct ComputedState(
    [property:JsonPropertyName("state")]
    object State
);