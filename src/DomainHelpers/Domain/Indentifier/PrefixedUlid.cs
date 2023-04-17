using DomainHelpers.Commons;
using DomainHelpers.Commons.Primitives;

namespace DomainHelpers.Domain.Indentifier;

public abstract record PrefixedUlid {
    public abstract string Prefix { get; }

    public Ulid Value { get; private set; }

    public string FullValue => $"{Prefix}{Value}";

    public PrefixedUlid(string id) {
        CheckIfValidAndSetUlid(id);
    }

    public PrefixedUlid() {
    }

    private void CheckIfValidAndSetUlid(string id) {
        if (id.StartsWith(Prefix) is false) {
            throw GeneralException.WithDisplayMessage($"Id must start with '{Prefix}'");
        }

        var ulidText = id.Replace(Prefix, "");
        if (Ulid.TryParse(ulidText, out var ulid)) {
            Value = ulid;
        }
        else {
            throw GeneralException.WithDisplayMessage($"Ulid '{ulidText}' is invalid ");
        }
    }

    public static T Parse<T>(string rawId) where T : PrefixedUlid, new() {
        var pid = new T();
        pid.CheckIfValidAndSetUlid(rawId);
        return pid;
    }

    public static T NewPrefixedUlid<T>() where T : PrefixedUlid, new() {
        var pid = new T();
        pid.Value = Ulid.NewUlid();
        return pid;
    }

    public sealed override string ToString() => FullValue;
}