namespace DomainHelpers.Commons.Enums;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class EnumDisplayNameAttribute : Attribute {
    public EnumDisplayNameAttribute(string displayName) {
        DisplayName = displayName;
    }

    public string DisplayName { get; init; }
}