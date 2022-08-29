using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Primitives;

public abstract class SearchResult {
    public static SearchResult<T> From<T>(
        int hitCount,
        int offset,
        int fetch,
        ImmutableArray<T> items
    ) => new SearchResult<T>(
        hitCount,
        offset,
        fetch,
        items
    );
}

public record SearchResult<T>(
    int HitCount,
    int Offset,
    int Fetch,
    ImmutableArray<T> Items) {
}
