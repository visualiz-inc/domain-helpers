﻿namespace DomainHelpers.Domain;

public interface IUnitOfWorkTransaction : IAsyncDisposable {
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}