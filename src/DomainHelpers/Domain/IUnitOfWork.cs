namespace DomainHelpers.Domain;

public interface IUnitOfWork {
    Task<IUnitOfWorkTransaction> BeginTransactionAsync();
}