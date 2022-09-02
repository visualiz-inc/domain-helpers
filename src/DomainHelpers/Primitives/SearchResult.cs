namespace DomainHelpers.Primitives {
    public abstract class SearchResult {
        public static SearchResult<T> From<T>(
            int hitCount,
            int offset,
            int fetch,
            ImmutableArray<T> items
        ) {
            return new(
                hitCount,
                offset,
                fetch,
                items
            );
        }
    }

    public record SearchResult<T> {
        public int HitCount { get; init; }
        public int Offset { get; init; }
        public int Fetch { get; init; }
        public ImmutableArray<T> Items { get; init; }

        public SearchResult(
                int hitCount,
                int offset,
                int fetch,
                ImmutableArray<T> items) {

        }
    }
}