using DomainHelpers.Domain.Indentifier;

namespace DomainHelpers.Commons;

public record ErrorId : PrefixedUlid {
    public const int TotalLength = 36;
    public const string Prefix = "exception";
    public const string Separator = "_";

    public override string PrefixWithSeparator => "exception_";

    public static ErrorId CreateNew() => DomainHelpers.Domain.Indentifier.PrefixedUlid.NewPrefixedUlid<ErrorId>();

    public static ErrorId Parse(string id) {
        return Parse<ErrorId>(id);
    }
}