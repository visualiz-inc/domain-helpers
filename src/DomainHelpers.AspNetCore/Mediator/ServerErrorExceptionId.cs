using DomainHelpers.Domain.Indentifier;

namespace DomainHelpers.AspNetCore.Mediator;
public record ServerErrorExceptionId : PrefixedUlid {
    public const int TotalLength = 39;
    public const string Prefix = "server_error";
    public const string Separator = "_";

    public override string PrefixWithSeparator => "server_error_";

    public static ServerErrorExceptionId CreateNew() => DomainHelpers.Domain.Indentifier.PrefixedUlid.NewPrefixedUlid<ServerErrorExceptionId>();

    public static ServerErrorExceptionId Parse(string id) {
        return DomainHelpers.Domain.Indentifier.PrefixedUlid.Parse<ServerErrorExceptionId>(id);
    }
}