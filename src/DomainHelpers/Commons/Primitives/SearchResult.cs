﻿using System.Diagnostics.CodeAnalysis;

namespace DomainHelpers.Primitives;

public abstract class SearchResult {
    public static SearchResult<T> From<T>(
        int total,
        int offset,
        int fetch,
        ImmutableArray<T> items
    ) {
        return new() {
            Total = total,
            Offset = offset,
            Fetch = fetch,
            Items = items,
        };
    }
}

public record SearchResult<T> {
    public required int Total { get; init; }

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
        Total = hitCount;
        Items = items;
        Fetch = fetch;
        Offset = offset;
    }
}
