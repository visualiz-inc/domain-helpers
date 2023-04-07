using Microsoft.EntityFrameworkCore.Storage;
using MoriFlocky.Domain.Common;

namespace MoriFlocky.Infrastructure.AzureSql;

public class UnitOfWorkTransaction : IUnitOfWorkTransaction {
    private readonly IDbContextTransaction _transaction;

    public UnitOfWorkTransaction(IDbContextTransaction transaction) {
        _transaction = transaction;
    }

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