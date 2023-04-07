using DomainHelpers.Commons.Primitives;

namespace DomainHelpers.Domain; 

public interface IQueryService<TEntity, TSearchOption> {
    Task<SearchResult<TEntity>> SearchAsync(int offset, int fetch, TSearchOption option);
}