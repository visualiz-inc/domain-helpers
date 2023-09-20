using DomainHelpers.Domain;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public class UnitOfWorkTransaction(IDbContextTransaction transaction) : IUnitOfWorkTransaction {
    private readonly IDbContextTransaction _transaction = transaction;

    public ValueTask DisposeAsync() {
        return _transaction.DisposeAsync();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default) {
        return _transaction.CommitAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default) {
        return _transaction.RollbackAsync(cancellationToken);
    }
}