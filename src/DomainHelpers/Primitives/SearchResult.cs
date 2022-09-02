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
    public record SearchResult<T>(
        int HitCount,
        int Offset,
        int Fetch,
        ImmutableArray<T> Items) {
        public SearchResult() { }
    }
}
