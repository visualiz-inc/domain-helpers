using DomainHelpers.Commons;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

/// <summary>
/// AzureSqlのデータへのアクセスの失敗を表します．
/// </summary>
public class GeneralDataAccessFailedException : GeneralException {
    public GeneralDataAccessFailedException(
        Exception exception,
        string? displayMessage = "データのアクセスに失敗しました。"
    ) : base(
        exception.Message,
        displayMessage,
        null,
        GeneralExceptionId.CreateNew(),
        exception
    ) { }
}