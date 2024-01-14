namespace DomainHelpers.Domain;

/// <summary>
/// オブジェクトの永続化の管理を行うリポジトリを表します。
/// </summary>
public interface IRepository<TEntity, TId> {
    /// <summary>
    /// 指定されたEntityIdに対応するEntityを非同期で検索します。
    /// </summary>
    /// <param name="EntityId">検索するTEntityのEntityId。</param>
    /// <returns>見つかったEntity、または見つからなかった場合はnull。</returns>
    Task<TEntity?> FindAsync(TId EntityId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> FindByIdsAsync(IEnumerable<TId?> ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// 新しいEntityをリポジトリに非同期で追加します。
    /// </summary>
    /// <param name="Entity">追加するEntityオブジェクト。</param>
    /// <returns>非同期操作を表すTask。</returns>
    Task AddAsync(TEntity Entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 既存のEntityの変更を非同期でリポジトリに保存します。
    /// </summary>
    /// <param name="Entity">保存するEntityオブジェクト。</param>
    /// <returns>非同期操作を表すTask。</returns>
    Task SaveAsync(TEntity Entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 指定されたEntityIdに対応するEntityを非同期で削除します。
    /// </summary>
    /// <param name="EntityId">削除するEntityのEntityId。</param>
    /// <returns>非同期操作を表すTask。</returns>
    Task RemoveAsync(TId EntityId, CancellationToken cancellationToken = default);
}