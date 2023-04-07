namespace DomainHelpers.Domain {
    namespace MoriFlocky.Domain.Common;

    public interface IQueryService<TEntity, TSearchOption> {
        Task<SearchResult<TEntity>> SearchAsync(int offset, int fetch, TSearchOption option);
    }
}