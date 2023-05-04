using DomainHelpers.Domain.Indentifier;

namespace DomainHelpers.Commons;

public record GeneralExceptionId : PrefixedUlid {
    public const int TotalLength = 36;
    public const string Prefix = "exception";
    public const string Separator = "_";

    public override string PrefixWithSeparator => "exception_";

    public static GeneralExceptionId CreateNew() => DomainHelpers.Domain.Indentifier.PrefixedUlid.NewPrefixedUlid<GeneralExceptionId>();

    public static GeneralExceptionId Parse(string id) {
        return DomainHelpers.Domain.Indentifier.PrefixedUlid.Parse<GeneralExceptionId>(id);
    }
}