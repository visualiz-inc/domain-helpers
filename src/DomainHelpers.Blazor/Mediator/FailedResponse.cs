using System.Text.Json;

namespace DomainHelpers.Blazor.Mediator;

public record FailedResponse {
    public required string Title { get; init; }

    public required int Status { get; init; }

    public required string Type { get; init; }

    public required string? Payload { get; init; }

    public required string EventId { get; init; }

    public required string? Exception { get; init; }

    public required ImmutableArray<string> ChildDetails { get; init; }

    public T? GetPayloadAs<T>() => Payload is null
        ? default
        : JsonSerializer.Deserialize<T>(Payload);
}

