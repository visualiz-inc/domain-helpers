using DomainHelpers.Commons;
using DomainHelpers.Commons.Primitives;

namespace MoriFlocky.Infrastructure.AzureSql;

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
        Ulid.NewUlid(),
        exception
    ) { }
}