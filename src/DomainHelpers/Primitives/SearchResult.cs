using System.Diagnostics.CodeAnalysis;

namespace DomainHelpers.Primitives;

public abstract class SearchResult {
    public static SearchResult<T> From<T>(
        int hitCount,
        int offset,
        int fetch,
        ImmutableArray<T> items
    ) {
        return new() {
            HitCount = hitCount, Offset = offset, Fetch = fetch, Items = items,
        };
    }
}

public record SearchResult<T> {
    public required int HitCount { get; init; }

    public required int Offset { get; init; }

    public required int Fetch { get; init; }

    public required ImmutableArray<T> Items { get; init; }

    public SearchResult() { }

    [SetsRequiredMembers]
    public SearchResult(
        int hitCount,
        int offset,
        int fetch,
        ImmutableArray<T> items
    ) {
        this.HitCount = hitCount;
        this.Items = items;
        this.Fetch = fetch;
        this.Offset = offset;
    }
}
