namespace DomainHelpers.Commons.Enums;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EnumDisplayNameAttribute(string displayName) : Attribute {
    public string DisplayName { get; init; } = displayName;
}